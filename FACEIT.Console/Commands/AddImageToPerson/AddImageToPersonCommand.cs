using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.AddImageToPerson
{
    internal class AddImageToPersonCommand : Command
    {
        public AddImageToPersonCommand() : base("add-image", "Add a persisted image to a person")
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

            var personIdOption = new Option<string>(
                name: "--person-id",
                description: "The id of the person.")
            {
                IsRequired = true,
            };
            personIdOption.AddAlias("-pn");
            AddOption(personIdOption);

            var imageFileOption = new Option<string>(
                name: "--image-file",
                description: "The file of the image to add to the person.")
            {
                IsRequired = true,
              };
            imageFileOption.AddAlias("-f");
            AddOption(imageFileOption);


            this.SetHandler(CommandHandler, groupIdOption, personIdOption, imageFileOption, new PersonsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, string personId, string imageFile, IPersonsManager personsManager)
        {
            if (File.Exists(imageFile) == false)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Image file {imageFile} not found.", ConsoleColor.Red);
                return;
            }

            ConsoleUtility.WriteLineWithTimestamp($"Adding image {imageFile} to person {personId} in the group {groupId}.");

            using var imageStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read);

            var response = await personsManager.AddImageToPersonAsync(groupId, personId, imageStream);

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
