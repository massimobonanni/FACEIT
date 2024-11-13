using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FACEIT.FaceService.Entities
{
    internal class Person
    {
        [JsonPropertyName("personId")] 
        public string Id { get; set; }

        [JsonPropertyName("name")] 
        public string Name { get; set; }

        [JsonPropertyName("userData")] 
        public string UserData { get; set; }

        [JsonPropertyName("persistedFaceIds")] 
        public IEnumerable<string>? PersistedFaceIds { get; set; }

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
