using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetPersons
{
    internal class GetPersonsCommand : Command
    {
        public GetPersonsCommand() : base("get-persons", "Returns the list of persons in a specific group")
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

            this.SetHandler(CommandHandler, groupIdOption, new PersonsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, IPersonsManager personsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Retrieving persons into the group {groupId}");

            var response = await personsManager.GetPersonsByGroupAsync(groupId);

            if (response.Success)
            {
                if (response.Data.Any())
                {
                    foreach (var person in response.Data)
                    {
                        ConsoleUtility.WriteLine($"Person ID: {person.Id}, Name: {person.Name}, Properties: {person.Properties.FormatProperties()}");
                    }
                }
                else
                {
                    ConsoleUtility.WriteLine("No persons found");
                }
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve persons from group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
