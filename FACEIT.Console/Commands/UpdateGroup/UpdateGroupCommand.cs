using FACEIT.Console.Binders;
using FACEIT.Console.Utilities;
using FACEIT.Core.Interfaces;
using System.CommandLine;

namespace FACEIT.Console.Commands.UpdateGroup
{
    internal class UpdateGroupCommand : Command
    {
        public UpdateGroupCommand() : base("update-group", "Update an existing group")
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

            var groupPropertiesOption = new Option<IEnumerable<string>>(
                name: "--group-properties",
                description: "The properties of the group in the form 'key:value'.")
            {
                IsRequired = false,
                AllowMultipleArgumentsPerToken = true
            };
            groupPropertiesOption.AddAlias("-gp");
            AddOption(groupPropertiesOption);


            this.SetHandler(CommandHandler,groupIdOption, groupNameOption, groupPropertiesOption, new GroupsManagerBinder(endpointOption, apiKeyOption));
        }

        private async Task CommandHandler(string groupId, string groupName, IEnumerable<string> groupProperties, IGroupsManager groupsManager)
        {
            ConsoleUtility.WriteLineWithTimestamp($"Updating group {groupName} with id {groupId}.");

            var properties = groupProperties.ToProperties();
            var response = await groupsManager.UpdateGroupAsync(groupId, groupName, properties);

            if (response.Success)
            {
                ConsoleUtility.WriteLineWithTimestamp($"Group {groupName} with id {groupId} updated successfully.", ConsoleColor.Green);
            }
            else
            {
                ConsoleUtility.WriteLineWithTimestamp($"Failed to update group {groupName} with id {groupId}. {response.Message}", ConsoleColor.Red);
            }
        }



    }


}
