using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetGroup
{
    internal class GetGroupCommand : Command
    {
        public GetGroupCommand() : base("get-group", "Returns the information of a group")
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

            this.SetHandler(CommandHandler,
                endpointOption, apiKeyOption, groupIdOption, new FacesManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string endpoint, string apiKey, string groupId, IFacesManager facesManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Retrieving group {groupId}");

            var response = await facesManager.GetGroupAsync(groupId);

            if (response.Success)
            {
                ConsoleUtility.WriteLine();
                ConsoleUtility.WriteLine($"Group ID: {response.Data.Id}, Name: {response.Data.Name}, Data: {response.Data.Data}");
                ConsoleUtility.WriteLine();
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve group. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
