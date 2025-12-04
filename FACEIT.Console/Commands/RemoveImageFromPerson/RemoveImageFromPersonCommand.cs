using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.RemoveImageFromPerson
{
    internal class RemoveImageFromPersonCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;
        private readonly Option<string> _personIdOption;
        private readonly Option<string> _imageIdOption;

        public RemoveImageFromPersonCommand(IFaceServiceFactory faceServiceFactory) : base("remove-image", "Remove a persisted image from a person")
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

            _imageIdOption = new Option<string>("--image-id", "-ii")
            {
                Description = "The id of the image.",
                Required = true
            };
            Options.Add(_imageIdOption);

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;
            var personId = parseResult.GetValue(_personIdOption)!;
            var imageId = parseResult.GetValue(_imageIdOption)!;

            var personsManager = _faceServiceFactory.CreatePersonsManager(endpoint, apiKey);

            ConsoleUtility.WriteLineWithTimestamp($"Removing image {imageId} to person {personId} in the group {groupId}.");

            var response = await personsManager.RemoveImageFromPersonAsync(groupId, personId, imageId, cancellationToken);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Image {imageId} removed successfully from person {personId} in the group {groupId}.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to remove image {imageId} from person {personId} in the group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
