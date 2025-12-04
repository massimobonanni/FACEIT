using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetGroup
{
    internal class GetGroupCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;

        public GetGroupCommand(IFaceServiceFactory faceServiceFactory) : base("get-group", "Returns the information of a group")
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

            ConsoleUtility.WriteLineWithTimestamp($"Retrieving group {groupId}");

            var response = await groupsManager.GetGroupAsync(groupId, cancellationToken);

            if (response.Success)
            {
                if (response.Data != null)
                {
                    ConsoleUtility.DisplayGroup(response.Data);
                }
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve group. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
