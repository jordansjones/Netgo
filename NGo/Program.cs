using System.Threading.Tasks;
using CommandLine;
using NGo.Commands;

namespace NGo
{
    class Program
    {

        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<
#if DEBUG
                    DebugCommand,
#endif
                    JumpCommand,
                    ListCommand,
                    RemoveCommand,
                    SaveCommand
                >(args)
                .MapResult
                (
#if DEBUG
                    (DebugCommand opts) => opts.ExecuteAsync(),
#endif
                    (JumpCommand opts) => opts.ExecuteAsync(),
                    (ListCommand opts) => opts.ExecuteAsync(),
                    (RemoveCommand opts) => opts.ExecuteAsync(),
                    (SaveCommand opts) => opts.ExecuteAsync(),
                    errs => Task.CompletedTask
                );
        }
    }
}
