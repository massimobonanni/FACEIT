using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.ClearAll
{
    internal class ClearAllCommand : Command
    {
        public ClearAllCommand() : base("clear", "Remove all the groups in the service")
        {
            var endpointOption = new Option<string>(
                name: "--endpoint",
                description: "The endpoint of Azure Face Service resource.")
            {
                IsRequired = false,
            };
            endpointOption.AddAlias("-e");
            AddOption(endpointOption);

            var apiKeyOption = new Option<string>(
                name: "--api-key",
                description: "The API key of Azure Face Service resource.")
            {
                IsRequired = false,
            };
            apiKeyOption.AddAlias("-k");
            AddOption(apiKeyOption);

            this.SetHandler(CommandHandler, new GroupsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler( IGroupsManager groupsManager)
        {
            ConsoleUtility.WriteLine("Are you sure you want to remove all the groups in the service? ([Y]yes/[N]no)");
            var userAnswer=System.Console.ReadLine();
            if (userAnswer.ToLower() != "y" && userAnswer.ToLower() != "yes")
            {
                ConsoleUtility.WriteLine("Operation cancelled.",ConsoleColor.Yellow);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Removing all the groups in the service.");

            var response = await groupsManager.ClearAllAsync();

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"All groups removed correctly.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to clear the service. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
