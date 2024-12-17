using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FACEIT.FaceService.Entities
{
    /// <summary>
    /// Represents a group entity with properties for large person group ID, name, and user data.
    /// </summary>
    internal class Group
    {
        /// <summary>
        /// Gets or sets the large person group ID.
        /// </summary>
        [JsonPropertyName("largePersonGroupId")]
        public string? LargePersonGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the user data associated with the group.
        /// </summary>
        [JsonPropertyName("userData")]
        public string? UserData { get; set; }

        /// <summary>
        /// Converts the current group instance to a core group instance.
        /// </summary>
        /// <returns>A core group instance with the same properties.</returns>
        internal Core.Entities.Group ToCoreGroup()
        {
            IDictionary<string, string>? properties = null;
            if (!string.IsNullOrEmpty(UserData))
            {
                try
                {
                    properties = JsonSerializer.Deserialize<IDictionary<string, string>>(UserData);
                }
                catch (JsonException)
                {
                    properties = null;
                }
            }

            return new Core.Entities.Group()
            {
                Id = LargePersonGroupId,
                Name = Name,
                Properties = properties
            };
        }
    }
}
