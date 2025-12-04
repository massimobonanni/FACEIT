using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetPersons
{
    internal class GetPersonsCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;

        public GetPersonsCommand(IFaceServiceFactory faceServiceFactory) : base("get-persons", "Returns the list of persons in a specific group")
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

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;

            var personsManager = _faceServiceFactory.CreatePersonsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Retrieving persons from the group {groupId}");

            var response = await personsManager.GetPersonsByGroupAsync(groupId, cancellationToken);

            if (response.Success)
            {
                if (response.Data != null && response.Data.Any())
                {
                    foreach (var person in response.Data)
                    {
                        ConsoleUtility.DisplayPerson(person);
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
