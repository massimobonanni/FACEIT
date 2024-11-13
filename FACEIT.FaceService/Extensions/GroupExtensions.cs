using FACEIT.FaceService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    internal static class GroupExtensions
    {
        public static string ToJson(this Group group, string recognitionModel)
        {
            var propertiesJson = 
                group.Properties != null ? 
                JsonSerializer.Serialize(group.Properties) : "{}";

            var wrappedObject = new { name = group.Name, userData = propertiesJson, recognitionModel=recognitionModel };
            return JsonSerializer.Serialize(wrappedObject);
        }
    }
}
