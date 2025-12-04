using FACEIT.Console.Services;
using FACEIT.Console.Utilities;
using System.CommandLine;

namespace FACEIT.Console.Commands.RecognizePerson
{
    internal class RecognizePersonCommand : Command
    {
        private readonly IFaceServiceFactory _faceServiceFactory;
        private readonly Option<string?> _endpointOption;
        private readonly Option<string?> _apiKeyOption;
        private readonly Option<string> _groupIdOption;
        private readonly Option<string> _imageFileOption;
        private readonly Option<int> _confidenceOption;

        public RecognizePersonCommand(IFaceServiceFactory faceServiceFactory) : base("recognize", "Recognize an image and get the person id")
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

            _imageFileOption = new Option<string>("--image-file", "-f")
            {
                Description = "The file of the image to add to the person.",
                Required = true
            };
            Options.Add(_imageFileOption);

            _confidenceOption = new Option<int>("--confidence", "-c")
            {
                Description = "The confidence of the recognition (in percentage, between 0 and 100). The default value is 75.",
                DefaultValueFactory = _ => 75
            };
            Options.Add(_confidenceOption);

            this.SetAction(CommandHandler);
        }

        private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
        {
            var endpoint = parseResult.GetValue(_endpointOption);
            var apiKey = parseResult.GetValue(_apiKeyOption);
            var groupId = parseResult.GetValue(_groupIdOption)!;
            var imageFile = parseResult.GetValue(_imageFileOption)!;
            var confidence = parseResult.GetValue(_confidenceOption);

            var personsManager = _faceServiceFactory.CreatePersonsManager(endpoint, apiKey);
            var faceRecognizer = _faceServiceFactory.CreateFaceRecognizer(endpoint, apiKey);

            if (File.Exists(imageFile) == false)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Image file {imageFile} not found.", ConsoleColor.Red);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Genarating temporary image from {imageFile}.");

            using var imageStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read);

            var tempImageResponse = await faceRecognizer.DetectAsync(imageStream, 60, cancellationToken);

            if (!tempImageResponse.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to create temporary image . {tempImageResponse.Message}", ConsoleColor.Red);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Recognizing image in group {groupId}.");

            var recognizeResponse = await faceRecognizer.RecognizeAsync(groupId, tempImageResponse.Data ?? string.Empty, confidence / 100.0f, cancellationToken);

            if (!recognizeResponse.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to recognize image. {recognizeResponse.Message}", ConsoleColor.Red);
                return;
            }

            if (recognizeResponse.Data == null || recognizeResponse.Data.Any() == false)
            {
                ConsoleUtility.WriteLineWithTimestamp("No person recognized.", ConsoleColor.Yellow);
                return;
            }

            ConsoleUtility.WriteLine("Recognized persons:");
            ConsoleUtility.WriteLine();
            foreach (var recognizedPerson in recognizeResponse.Data)
            {
                var personResponse = await personsManager.GetPersonAsync(groupId, recognizedPerson.Id ?? string.Empty, cancellationToken);
                if (personResponse.Data != null)
                {
                    ConsoleUtility.DisplayRecognizedPerson(recognizedPerson, personResponse.Data);
                }
            }
        }
    }
}
