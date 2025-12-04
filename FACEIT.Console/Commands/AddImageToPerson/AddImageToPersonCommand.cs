using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.AddImageToPerson
{
    internal class AddImageToPersonCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;
        private readonly Option<string> _personIdOption;
        private readonly Option<string> _imageFileOption;

        public AddImageToPersonCommand(IFaceServiceFactory faceServiceFactory) : base("add-image", "Add a persisted image to a person")
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

            _imageFileOption = new Option<string>("--image-file", "-f")
            {
                Description = "The file of the image to add to the person.",
                Required = true
            };
            Options.Add(_imageFileOption);

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;
            var personId = parseResult.GetValue(_personIdOption)!;
            var imageFile = parseResult.GetValue(_imageFileOption)!;

            var personsManager = _faceServiceFactory.CreatePersonsManager(endpoint, apiKey);

            if (File.Exists(imageFile) == false)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Image file {imageFile} not found.", ConsoleColor.Red);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Adding image {imageFile} to person {personId} in the group {groupId}.");

            using var imageStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read);

            var response = await personsManager.AddImageToPersonAsync(groupId, personId, imageStream, cancellationToken);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Image added successfully with id {response.Data} to person {personId} in the group {groupId}.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to add image to person {personId} in the group {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }
    }
}
