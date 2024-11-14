using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetPerson
{
    internal class GetPersonCommand : Command
    {
        public GetPersonCommand() : base("get-person", "Returns a person in a specific group")
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

            var personIdOption = new Option<string>(
                name: "--person-id",
                description: "The id of the person.")
            {
                IsRequired = true,
            };
            personIdOption.AddAlias("-pi");
            AddOption(personIdOption);

            this.SetHandler(CommandHandler, groupIdOption, personIdOption, new PersonsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, string personId, IPersonsManager personsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Retrieving person {personId} from the group {groupId}");

            var response = await personsManager.GetPersonAsync(groupId,personId);

            if (response.Success)
            {
                ConsoleUtility.DisplayPerson(response.Data);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve persons from group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
