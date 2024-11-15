using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.UpdatePerson
{
    internal class UpdatePersonCommand : Command
    {
        public UpdatePersonCommand() : base("update-person", "Update an existing person in a group")
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

            var personNameOption = new Option<string>(
                name: "--person-name",
                description: "The name of the person.")
            {
                IsRequired = true,
            };
            personNameOption.AddAlias("-pn");
            AddOption(personNameOption);

            var personPropertiesOption = new Option<IEnumerable<string>>(
                name: "--person-properties",
                description: "The properties of the group in the form 'key:value'.")
            {
                IsRequired = false,
                AllowMultipleArgumentsPerToken = true
            };
            personPropertiesOption.AddAlias("-p");
            AddOption(personPropertiesOption);


            this.SetHandler(CommandHandler, groupIdOption, personIdOption, personNameOption, personPropertiesOption, new PersonsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, string personId,  string personName, IEnumerable<string> personProperties, IPersonsManager personsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Updating person id {personId} in group {groupId}.");

            var properties = personProperties.ToProperties();
            var response = await personsManager.UpdatePersonAsync(groupId, personId,personName, properties);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Person {personName} with id {personId} updated successfully.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to update person {personId}. {response.Message}", ConsoleColor.Red);
            }
        }

    }


}
