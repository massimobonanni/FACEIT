using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Entities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.RecognizePerson
{
    internal class RecognizePersonCommand : Command
    {
        public RecognizePersonCommand() : base("recognize", "Recognize an image and get the person id")
        {
            var endpointOption = new Option<string>(
                name: "--endpoint",
                description: "The endpoint of Azure Face Service resource.")
            {
                IsRequired = false,
            };
            endpointOption.AddAlias("-e");
            AddOption(endpointOption);

            var apiKeyOption = new Option<string>(
                name: "--api-key",
                description: "The API key of Azure Face Service resource.")
            {
                IsRequired = false,
            };
            apiKeyOption.AddAlias("-k");
            AddOption(apiKeyOption);

            var groupIdOption = new Option<string>(
                name: "--group-id",
                description: "The id of the group.")
            {
                IsRequired = true,
            };
            groupIdOption.AddAlias("-gi");
            AddOption(groupIdOption);

            var imageFileOption = new Option<string>(
                name: "--image-file",
                description: "The file of the image to add to the person.")
            {
                IsRequired = true,
            };
            imageFileOption.AddAlias("-f");
            AddOption(imageFileOption);

            var confidenceOption = new Option<int>(
                name: "--confidence",
                description: "The confidence of the recognition (in percentage, between 0 and 100). The dafault value is 75.")
            {
                IsRequired = false,
            };
            confidenceOption.AddAlias("-c");
            confidenceOption.SetDefaultValue(75);
            AddOption(confidenceOption);

            this.SetHandler(CommandHandler, groupIdOption, imageFileOption, confidenceOption, new PersonsManagerBinder(endpointOption, apiKeyOption), new FaceRecognizerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, string imageFile, int confidence, IPersonsManager personsManager, IFaceRecognizer faceRecognizer)
        {
            if (File.Exists(imageFile) == false)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Image file {imageFile} not found.", ConsoleColor.Red);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Genarating temporary image from {imageFile}.");

            using var imageStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read);

            var tempImageResponse = await faceRecognizer.DetectAsync(imageStream, 60);

            if (!tempImageResponse.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to create temporary image . {tempImageResponse.Message}", ConsoleColor.Red);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Recognizing image in group {groupId}.");

            var recognizeResponse = await faceRecognizer.RecognizeAsync(groupId, tempImageResponse.Data, confidence / 100.0f);

            if (!recognizeResponse.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to recognize image. {recognizeResponse.Message}", ConsoleColor.Red);
                return;
            }

            if (recognizeResponse.Data.Any() == false)
            {
                ConsoleUtility.WriteLineWithTimestamp("No person recognized.", ConsoleColor.Yellow);
                return;
            }

            ConsoleUtility.WriteLine("Recognized persons:");
            ConsoleUtility.WriteLine();
            foreach (var recognizedPerson in recognizeResponse.Data)
            {
                var personResponse = await personsManager.GetPersonAsync(groupId, recognizedPerson.Id);
                ConsoleUtility.DisplayRecognizedPerson(recognizedPerson, personResponse.Data);
            }
        }
    }
}
