using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetTrainingStatus
{
    internal class GetTrainingStatusCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;

        public GetTrainingStatusCommand(IFaceServiceFactory faceServiceFactory) : base("train-status", "Retrieve the status of a trainig for a group")
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

            var groupsManager = _faceServiceFactory.CreateGroupsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Retrieving training status for group {groupId}");

            var response = await groupsManager.GetTrainingStatusAsync(groupId, cancellationToken);

            if (response.Success)
            {
                if (response.Data != null)
                {
                    ConsoleUtility.DisplayTrainingStatus(response.Data);
                }
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve training status for group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
