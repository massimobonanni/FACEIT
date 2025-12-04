using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.CreatePerson
{
    internal class CreatePersonCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;
        private readonly Option<string> _personNameOption;
        private readonly Option<IEnumerable<string>?> _personPropertiesOption;

        public CreatePersonCommand(IFaceServiceFactory faceServiceFactory) : base("create-person", "Create a person in a group")
        {
            _faceServiceFactory = faceServiceFactory;

            _endpointOption = new Option<string?>("--endpoint", "-e")
            {
                Description = "The endpoint of Azure Face Service resource."
            };
            Options.Add(_endpointOption);

            _apiKeyOption = new Option<string?>("--api-key", "-k")
            {
                Description = "The API key of Azure Face Service resource."
            };
            Options.Add(_apiKeyOption);

            _groupIdOption = new Option<string>("--group-id", "-gi")
            {
                Description = "The id of the group.",
                Required = true
            };
            Options.Add(_groupIdOption);

            _personNameOption = new Option<string>("--person-name", "-pn")
            {
                Description = "The name of the person.",
                Required = true
            };
            Options.Add(_personNameOption);

            _personPropertiesOption = new Option<IEnumerable<string>?>("--person-properties", "-pp")
            {
                Description = "The properties of the person in the form 'key:value'.",
                AllowMultipleArgumentsPerToken = true
            };
            Options.Add(_personPropertiesOption);

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;
            var personName = parseResult.GetValue(_personNameOption)!;
            var personProperties = parseResult.GetValue(_personPropertiesOption);

            var personsManager = _faceServiceFactory.CreatePersonsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Creating person {personName} in the group {groupId}.");

            var properties = (personProperties ?? Enumerable.Empty<string>()).ToProperties();
            var response = await personsManager.CreatePersonAsync(groupId, personName, properties, cancellationToken);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Person {personName} created successfully with id {response.Data?.Id}.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to create person {personName} in the group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
