using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace NGo.Commands
{
    [Verb("remove", HelpText = "Removes an alias")]
    internal sealed class RemoveCommand : BaseCommand
    {
        [Value(0, MetaName = "alias", HelpText = "The name of the path to remove")]
        public string AliasName { get; set; }
        
        [Option('c', "config", Required = false, HelpText = "Path to where the configuration is stored")]
        public override FileInfo ConfigFile { get; set; }

        public override async Task ExecuteAsync()
        {
            if (string.IsNullOrWhiteSpace(AliasName)) return;

            var configData = await ReadConfig(ConfigFile);
            var key = configData.Keys.FirstOrDefault(x => string.Equals(x, AliasName, StringComparison.OrdinalIgnoreCase));
            if (key != null)
            {
                configData.Remove(key);
                await WriteConfig(configData);
            }
        }
    }
}