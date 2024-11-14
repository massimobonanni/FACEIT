using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Azure.AI.Vision.Face
{
    internal static class LargePersonGroupPersonExtensions
    {
        internal static Person ToCorePerson(this LargePersonGroupPerson source)
        {
            IDictionary<string, string>? properties = null;
            if (!string.IsNullOrEmpty(source.UserData))
            {
                properties = JsonSerializer.Deserialize<IDictionary<string, string>>(source.UserData);
            }

            IEnumerable<string> persistedfaceIds = null;
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
