using FACEIT.Console.Commands.AddImageToPerson;
using FACEIT.Console.Commands.ClearAll;
using FACEIT.Console.Commands.CreateGroup;
using FACEIT.Console.Commands.CreatePerson;
using FACEIT.Console.Commands.GetGroup;
using FACEIT.Console.Commands.GetGroups;
using FACEIT.Console.Commands.GetPersons;
using FACEIT.Console.Commands.GetTrainingStatus;
using FACEIT.Console.Commands.RecognizePerson;
using FACEIT.Console.Commands.RemoveGroupCommand;
using FACEIT.Console.Commands.RemoveImageFromPerson;
using FACEIT.Console.Commands.TrainGroup;
using FACEIT.Console.Commands.UpdateGroup;
using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.CommandLine;


namespace FACEIT.Console
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            ConsoleUtility.WriteApplicationBanner();

            // Set up dependency injection
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddConsole());
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IFaceServiceFactory, FaceServiceFactory>();

            var serviceProvider = services.BuildServiceProvider();
            var faceServiceFactory = serviceProvider.GetRequiredService<IFaceServiceFactory>();

            var rootCommand = new RootCommand("Console for managing FACEIT features");

            rootCommand.Subcommands.Add(new AddImageToPersonCommand(faceServiceFactory));

            rootCommand.Subcommands.Add(new ClearAllCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new CreateGroupCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new CreatePersonCommand(faceServiceFactory));

            rootCommand.Subcommands.Add(new GetGroupCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new GetGroupsCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new GetPersonsCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new GetTrainingStatusCommand(faceServiceFactory));

            rootCommand.Subcommands.Add(new RecognizePersonCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new RemoveGroupCommand(faceServiceFactory));
            rootCommand.Subcommands.Add(new RemoveImageFromPersonCommand(faceServiceFactory));

            rootCommand.Subcommands.Add(new TrainGroupCommand(faceServiceFactory));
            
            rootCommand.Subcommands.Add(new UpdateGroupCommand(faceServiceFactory));

            return await rootCommand.Parse(args).InvokeAsync();
        }
    }
}
