using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;

namespace NGo.Commands
{
    [Verb("jump", HelpText = "Output the shell command to change directory to the path for the specified alias")]
    internal sealed class JumpCommand : BaseCommand
    {

        [Value(0, MetaName = "alias", HelpText = "The name of the path to change directories to")]
        public string AliasName { get; set; }
        
        [Option('c', "config", Required = false, HelpText = "Path to where the configuration is stored")]
        public override FileInfo ConfigFile { get; set; }

        public override async Task ExecuteAsync()
        {
            if (string.IsNullOrWhiteSpace(AliasName)) return;

            var configData = await ReadConfig(ConfigFile);
            if (configData.TryGetValue(AliasName, out var dir))
            {
                Console.Write("cd \"{0}\"", dir);
            }
        }
    }
}