using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace NGo.Commands
{
    [Verb("list", HelpText = "List all of the path aliases")]
    internal sealed class ListCommand : BaseCommand
    {
        [Option("completion", HelpText = "List aliases for shell completion")]
        public bool ForCompletion { get; set; }
        
        [Option('c', "config", Required = false, HelpText = "Path to where the configuration is stored")]
        public override FileInfo ConfigFile { get; set; }

        public override async Task ExecuteAsync()
        {
            var configData = (await ReadConfig(ConfigFile)).OrderBy(x => x.Key);
            if (ForCompletion)
            {
                Console.WriteLine(string.Join(" ", configData.Select(x => x.Key)));
            }
            else
            {
                var items = configData.ToList();
                var longest = items.Aggregate(0, (len, val) => val.Key.Length > len ? val.Key.Length : len) + 1;
                var format = $"{{0, -{longest}}} : {{1}}";
                var lines = items.Select(x => string.Format(format, x.Key, x.Value)).ToList();
                var divider = new string('-', lines.Select(x => x.Length).DefaultIfEmpty(0).Max());
                for (var i = 0; i < lines.Count; i++)
                {
                    if (i > 0)
                    {
                        Console.WriteLine(divider);
                    }
                    Console.WriteLine(lines[i]);
                }
            }
        }
    }
}