using AOAIFineTuning.FTFilesGenerator.Commands.GenerateJSONLCommand;
using FACEIT.Console.Utilities;
using System.CommandLine;


namespace FACEIT.Console
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            ConsoleUtility.WriteApplicationBanner();

            var rootCommand = new RootCommand("Console for managing FACEIT stuff");
            
            rootCommand.AddCommand(new CreateGroupCommand());
            
            return await rootCommand.InvokeAsync(args);
        }
    }
}
