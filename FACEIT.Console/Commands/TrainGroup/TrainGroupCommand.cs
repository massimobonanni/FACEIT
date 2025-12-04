using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.TrainGroup
{
    internal class TrainGroupCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;

        public TrainGroupCommand(IFaceServiceFactory faceServiceFactory) : base("train-group", "Start the training of a specific group")
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

            ConsoleUtility.WriteLineWithTimestamp($"Starting Train group {groupId}");

            var response = await groupsManager.TrainGroupAsync(groupId, cancellationToken);

            if (response.Success)
            {
                ConsoleUtility.WriteLine($"Training started for group {groupId}");
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to start training for group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
