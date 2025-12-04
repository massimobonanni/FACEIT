using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Azure.AI.Vision.Face
{
    /// <summary>
    /// Provides extension methods for converting <see cref="LargePersonGroupPerson"/> objects to <see cref="Person"/> objects.
    /// </summary>
    internal static class LargePersonGroupPersonExtensions
    {
        /// <summary>
        /// Converts a <see cref="LargePersonGroupPerson"/> object to a <see cref="Person"/> object.
        /// </summary>
        /// <param name="source">The <see cref="LargePersonGroupPerson"/> object to convert.</param>
        /// <returns>A <see cref="Person"/> object that represents the converted <see cref="LargePersonGroupPerson"/>.</returns>
        internal static Person ToCorePerson(this LargePersonGroupPerson source)
        {
            IDictionary<string, string>? properties = null;
            if (!string.IsNullOrEmpty(source.UserData))
            {
                properties = JsonSerializer.Deserialize<IDictionary<string, string>>(source.UserData);
            }

            IEnumerable<string>? persistedfaceIds = null;
            if (source.PersistedFaceIds != null)
            {
                persistedfaceIds = source.PersistedFaceIds.Select(p => p.ToString());
            }

            return new Person()
            {
                Id = source.PersonId.ToString(),
                Name = source.Name,
                Properties = properties,
                PersistedFaceIds = persistedfaceIds
            };
        }
    }
}
