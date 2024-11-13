using FACEIT.Console.Commands.AddImageToPerson;
using FACEIT.Console.Commands.CreateGroup;
using FACEIT.Console.Commands.CreatePerson;
using FACEIT.Console.Commands.GetGroup;
using FACEIT.Console.Commands.GetGroups;
using FACEIT.Console.Commands.GetPersons;
using FACEIT.Console.Commands.RemoveGroupCommand;
using FACEIT.Console.Utilities;
using System.CommandLine;


namespace FACEIT.Console
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            ConsoleUtility.WriteApplicationBanner();

            var rootCommand = new RootCommand("Console for managing FACEIT features");
            
            rootCommand.AddCommand(new CreateGroupCommand());
            rootCommand.AddCommand(new GetGroupsCommand());
            rootCommand.AddCommand(new GetGroupCommand());
            rootCommand.AddCommand(new RemoveGroupCommand());

            rootCommand.AddCommand(new CreatePersonCommand());
            rootCommand.AddCommand(new GetPersonsCommand());
            rootCommand.AddCommand(new AddImageToPersonCommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
