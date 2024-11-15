using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.RemoveImageFromPerson
{
    internal class RemoveImageFromPersonCommand : Command
    {
        public RemoveImageFromPersonCommand() : base("remove-image", "Remove a persisted image from a person")
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
            personIdOption.AddAlias("-pi");
            AddOption(personIdOption);

            var imageIdOption = new Option<string>(
                name: "--image-id",
                description: "The id of the image.")
            {
                IsRequired = true,
            };
            imageIdOption.AddAlias("-ii");
            AddOption(imageIdOption);


            this.SetHandler(CommandHandler, groupIdOption, personIdOption, imageIdOption, new PersonsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, string personId, string imageId, IPersonsManager personsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Removing image {imageId} to person {personId} in the group {groupId}.");

            var response = await personsManager.RemoveImageFromPersonAsync(groupId, personId, imageId);

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
