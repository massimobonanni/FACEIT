using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.CreatePerson
{
    internal class CreatePersonCommand : Command
    {
        public CreatePersonCommand() : base("create-person", "Create a person in a group")
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
                description: "The properties of the person in the form 'key:value'.")
            {
                IsRequired = false,
                AllowMultipleArgumentsPerToken = true
            };
            personPropertiesOption.AddAlias("-pp");
            AddOption(personPropertiesOption);


            this.SetHandler(CommandHandler, groupIdOption, personNameOption, personPropertiesOption, new PersonsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler( string groupId, string personName, IEnumerable<string> personProperties, IPersonsManager personsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Creating person {personName} in the group {groupId}.");

            var properties = personProperties.ToProperties();
            var response = await personsManager.CreatePersonAsync(groupId, personName, properties);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Person {personName} created successfully with id {response.Data.Id}.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to create person {personName} in the group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
