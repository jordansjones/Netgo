using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace NGo.Commands
{
#if DEBUG
    [Verb("debug", HelpText = "Simple testing helper")]
    internal sealed class DebugCommand : BaseCommand
    {

        [Option('b', "bool", Default = false, HelpText = "Simple boolean argument")]
        public bool BoolValue { get; set; }

        [Option('i', "int", Default = 0, HelpText = "Simple int argument")]
        public int IntValue { get; set; }
        
        [Option('c', "config", Required = false, HelpText = "Path to where the configuration is stored")]
        public override FileInfo ConfigFile { get; set; }

        public override Task ExecuteAsync()
        {
            Console.WriteLine("Default ConfigFile: {0}", DefaultConfig.FullName);
            Console.WriteLine("Passed ConfigFile: {0}", ConfigFile?.FullName);
            Console.WriteLine("Bool Argument: {0}", BoolValue);
            Console.WriteLine("Int Argument: {0}", IntValue);

            foreach (var specialFolder in Enum.GetValues(typeof(Environment.SpecialFolder))
                .Cast<Environment.SpecialFolder>())
            {
                Console.WriteLine("{0}: {1}", specialFolder, Environment.GetFolderPath(specialFolder));
            }

            Console.WriteLine("Platform: {0}", Environment.OSVersion.Platform);
            return Task.CompletedTask;
        }
    }
#endif
}