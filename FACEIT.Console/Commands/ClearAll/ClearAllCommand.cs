using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.ClearAll
{
    internal class ClearAllCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;

        public ClearAllCommand(IFaceServiceFactory faceServiceFactory) : base("clear", "Remove all the groups in the service")
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

            ConsoleUtility.WriteLine("Are you sure you want to remove all the groups in the service? ([Y]yes/[N]no)");
            var userAnswer = System.Console.ReadLine();
            if (userAnswer?.ToLower() != "y" && userAnswer?.ToLower() != "yes")
            {
                ConsoleUtility.WriteLine("Operation cancelled.", ConsoleColor.Yellow);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Removing all the groups in the service.");

            var response = await groupsManager.ClearAllAsync(cancellationToken);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"All groups removed correctly.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to clear the service. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
