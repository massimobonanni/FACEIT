using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetPerson
{
    internal class GetPersonCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;
        private readonly Option<string> _personIdOption;

        public GetPersonCommand(IFaceServiceFactory faceServiceFactory) : base("get-person", "Returns a person in a specific group")
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

            _personIdOption = new Option<string>("--person-id", "-pi")
            {
                Description = "The id of the person.",
                Required = true
            };
            Options.Add(_personIdOption);

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;
            var personId = parseResult.GetValue(_personIdOption)!;

            var personsManager = _faceServiceFactory.CreatePersonsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Retrieving person {personId} from the group {groupId}");

            var response = await personsManager.GetPersonAsync(groupId, personId, cancellationToken);

            if (response.Success)
            {
                if (response.Data != null)
                {
                    ConsoleUtility.DisplayPerson(response.Data);
                }
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve persons from group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
