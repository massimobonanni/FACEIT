using Azure.AI.Vision.Face;
using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Azure.AI.Vision.Face
{
    internal static class LargePersonGroupExtensions
    {
        internal static Group ToCoreGroup(this LargePersonGroup source)
        {
            IDictionary<string, string>? properties = null;
            if (!string.IsNullOrEmpty(source.UserData))
            {
                properties = JsonSerializer.Deserialize<IDictionary<string, string>>(source.UserData);
            }
            return new Group()
            {
                Id = source.LargePersonGroupId,
                Name = source.Name,
                Properties = properties
            };
        }
    }
}
