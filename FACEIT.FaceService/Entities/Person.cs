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
    /// Represents a person with an ID, name, user data, and persisted face IDs.
    /// </summary>
    internal class Person
    {
        /// <summary>
        /// Gets or sets the unique identifier for the person.
        /// </summary>
        [JsonPropertyName("personId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user data associated with the person.
        /// </summary>
        [JsonPropertyName("userData")]
        public string UserData { get; set; }

        /// <summary>
        /// Gets or sets the collection of persisted face IDs associated with the person.
        /// </summary>
        [JsonPropertyName("persistedFaceIds")]
        public IEnumerable<string>? PersistedFaceIds { get; set; }

        /// <summary>
        /// Converts the current instance to a Core.Entities.Person object.
        /// </summary>
        /// <returns>A Core.Entities.Person object with the same data.</returns>
        internal Core.Entities.Person ToCorePerson()
        {
            IDictionary<string, string>? properties = null;
            if (!string.IsNullOrEmpty(UserData))
            {
                properties = JsonSerializer.Deserialize<IDictionary<string, string>>(UserData);
            }

            return new Core.Entities.Person()
            {
                Id = Id,
                Name = Name,
                Properties = properties,
                PersistedFaceIds = PersistedFaceIds
            };
        }
    }
}
