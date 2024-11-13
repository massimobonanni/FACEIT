using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    internal static class PersonExtensions
    {
        public static string ToJson(this Person person)
        {
            var propertiesJson =
                person.Properties != null ? 
                JsonSerializer.Serialize(person.Properties) : "{}";
            var wrappedObject = new { name = person.Name, userData = propertiesJson };
            return JsonSerializer.Serialize(wrappedObject);
        }
    }
}
