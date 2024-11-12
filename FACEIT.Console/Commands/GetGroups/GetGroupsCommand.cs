using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetGroups
{
    internal class GetGroupsCommand : Command
    {
        public GetGroupsCommand() : base("get-groups", "Returns the list of existing groups")
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

            this.SetHandler(CommandHandler,
                endpointOption, apiKeyOption, new FacesManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string endpoint, string apiKey, IFacesManager facesManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Retrieving groups");

            var response = await facesManager.GetGroupsAsync();

            if (response.Success)
            {
                if (response.Data.Any())
                {
                    foreach (var group in response.Data)
                    {
                        ConsoleUtility.WriteLine($"Group ID: {group.Id}, Name: {group.Name}, Data: {group.Data}");
                    }
                }
                else
                {
                    ConsoleUtility.WriteLine("No groups found");
                }
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve groups. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
