using FACEIT.Console.Commands.CreateGroup;
using FACEIT.Console.Commands.GetGroups;
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
            rootCommand.AddCommand(new GetGroupsCommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
