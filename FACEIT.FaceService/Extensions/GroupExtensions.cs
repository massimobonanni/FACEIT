using Azure.AI.Vision.Face;
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
        public static string ToJson(this Group group, FaceRecognitionModel recognitionModel)
        {
            var propertiesJson = group.PropertiesToJson();

            var wrappedObject = new { name = group.Name, userData = propertiesJson, recognitionModel=recognitionModel.ToString() };
            return JsonSerializer.Serialize(wrappedObject);
        }

        public static string PropertiesToJson(this Group group)
        {
            var propertiesJson =
                    group.Properties != null ?
                    JsonSerializer.Serialize(group.Properties) : "{}";
            return propertiesJson;
        }
    }
}
