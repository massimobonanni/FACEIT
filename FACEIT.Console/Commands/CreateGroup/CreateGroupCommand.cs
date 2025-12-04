using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.CreateGroup
{
    internal class CreateGroupCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;
        private readonly Option<string> _groupNameOption;
        private readonly Option<IEnumerable<string>?> _groupPropertiesOption;

        public CreateGroupCommand(IFaceServiceFactory faceServiceFactory) : base("create-group", "Create a new group")
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

            _groupNameOption = new Option<string>("--group-name", "-gn")
            {
                Description = "The name of the group.",
                Required = true
            };
            Options.Add(_groupNameOption);

            _groupPropertiesOption = new Option<IEnumerable<string>?>("--group-properties", "-gp")
            {
                Description = "The properties of the group in the form 'key:value'.",
                AllowMultipleArgumentsPerToken = true
            };
            Options.Add(_groupPropertiesOption);

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;
            var groupName = parseResult.GetValue(_groupNameOption)!;
            var groupProperties = parseResult.GetValue(_groupPropertiesOption);

            var groupsManager = _faceServiceFactory.CreateGroupsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Creating group {groupName} with id {groupId}.");

            var properties = (groupProperties ?? Enumerable.Empty<string>()).ToProperties();
            var response = await groupsManager.CreateGroupAsync(groupId, groupName, properties, cancellationToken);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Group {groupName} with id {groupId} created successfully.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to create group {groupName} with id {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
