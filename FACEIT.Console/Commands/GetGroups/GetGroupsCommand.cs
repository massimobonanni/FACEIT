using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.GetGroups
{
    internal class GetGroupsCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;

        public GetGroupsCommand(IFaceServiceFactory faceServiceFactory) : base("get-groups", "Returns the list of existing groups")
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

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);

            var groupsManager = _faceServiceFactory.CreateGroupsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Retrieving groups");

            var response = await groupsManager.GetGroupsAsync(cancellationToken);

            if (response.Success)
            {
                if (response.Data != null && response.Data.Any())
                {
                    foreach (var group in response.Data)
                    {
                        ConsoleUtility.DisplayGroup(group);
                    }
                }
                else
                {
                    ConsoleUtility.WriteLine("No groups found");
                }
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to retrieve groups. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
