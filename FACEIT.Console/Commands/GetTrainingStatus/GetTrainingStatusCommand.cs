using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetTrainingStatus
{
    internal class GetTrainingStatusCommand : Command
    {
        public GetTrainingStatusCommand() : base("train-status", "Retrieve the status of a trainig for a group")
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

            var groupIdOption = new Option<string>(
                name: "--group-id",
                description: "The id of the group.")
            {
                IsRequired = true,
            };
            groupIdOption.AddAlias("-gi");
            AddOption(groupIdOption);

            this.SetHandler(CommandHandler,groupIdOption, new GroupsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, IGroupsManager groupsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Retrieving training status for group {groupId}");

            var response = await groupsManager.GetTrainingStatusAsync(groupId);

            if (response.Success)
            {
                ConsoleUtility.DisplayTrainingStatus(response.Data);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve training status for group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }

    }

}
