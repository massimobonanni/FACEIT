using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace AOAIFineTuning.FTFilesGenerator.Commands.GenerateJSONLCommand
{
    internal class CreateGroupCommand : Command
    {
        public CreateGroupCommand() : base("create-group", "Generate JSONL file from csv file")
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

            var groupNameOption = new Option<string>(
                name: "--group-name",
                description: "The name of the group.")
            {
                IsRequired = true,
            };
            groupNameOption.AddAlias("-gn");
            AddOption(groupNameOption);

            var groupDataOption = new Option<string>(
                name: "--group-data",
                description: "The user data of the group.")
            {
                IsRequired = false,
            };
            groupDataOption.AddAlias("-gd");
            AddOption(groupDataOption);


            this.SetHandler(CommandHandler,
                endpointOption, apiKeyOption, groupIdOption, groupNameOption, groupDataOption, new FacesManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string endpoint, string apiKey, string groupId, string groupName, string groupData, IFacesManager facesManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Creating group {groupName} with id {groupId}.");

            var response = await facesManager.CreateGroupAsync(groupId, groupName, groupData);

            if (response.Success)
            { 
                ConsoleUtility.WriteLineWithTimestamp($"Group {groupName} with id {groupId} created successfully.",ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to create group {groupName} with id {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
