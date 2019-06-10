using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;

namespace NGo.Commands
{

    [Verb("save", HelpText = "Saves a path to an alias")]
    internal sealed class SaveCommand : BaseCommand
    {

        [Option('d', "dir", Required = false, HelpText = "Use this path instead of the current directory")]
        public DirectoryInfo Directory { get; set; }

        [Value(0, MetaName = "alias", HelpText = "The name which to save the path under")]
        public string AliasName { get; set; }
        
        [Option('c', "config", Required = false, HelpText = "Path to where the configuration is stored")]
        public override FileInfo ConfigFile { get; set; }

        public override async Task ExecuteAsync()
        {
            if (Directory == null)
            {
                Directory = new DirectoryInfo(Environment.CurrentDirectory);
            }
            var configData = await ReadConfig(ConfigFile);
            configData[AliasName] = Directory.FullName;
            await WriteConfig(configData);
        }
    }
}