using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FACEIT.FaceService.Entities
{
    internal class Group
    {
        [JsonPropertyName("personGroupId")]
        public string? PersonGroupId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("userData")]
        public string? UserData { get; set; }

        internal Core.Entities.Group ToCoreGroup()
        {
            IDictionary<string, string>? properties = null;
            if (!string.IsNullOrEmpty(UserData))
            {
                properties = JsonSerializer.Deserialize<IDictionary<string, string>>(UserData);
            }

            return new Core.Entities.Group()
            {
                Id = PersonGroupId,
                Name = Name,
                Properties = properties
            };
        }
    }
}
