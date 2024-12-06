using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    /// <summary>
    /// Provides extension methods for the <see cref="Person"/> class.
    /// </summary>
    internal static class PersonExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="Person"/> object to its JSON representation.
        /// </summary>
        /// <param name="person">The <see cref="Person"/> object to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Person"/> object.</returns>
        public static string ToJson(this Person person)
        {
            var propertiesJson = person.PropertiesToJson();
            var wrappedObject = new { name = person.Name, userData = propertiesJson };
            return JsonSerializer.Serialize(wrappedObject);
        }

        /// <summary>
        /// Converts the properties of the specified <see cref="Person"/> object to their JSON representation.
        /// </summary>
        /// <param name="person">The <see cref="Person"/> object whose properties are to be converted.</param>
        /// <returns>A JSON string representation of the properties of the <see cref="Person"/> object.</returns>
        public static string PropertiesToJson(this Person person)
        {
            var propertiesJson =
                    person.Properties != null ?
                    JsonSerializer.Serialize(person.Properties) : "{}";
            return propertiesJson;
        }
    }
}
