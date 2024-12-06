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
    /// <summary>
    /// Provides extension methods for the <see cref="Group"/> class.
    /// </summary>
    internal static class GroupExtensions
    {
        /// <summary>
        /// Converts the <see cref="Group"/> object to a JSON string.
        /// </summary>
        /// <param name="group">The <see cref="Group"/> object to convert.</param>
        /// <param name="recognitionModel">The face recognition model to include in the JSON.</param>
        /// <returns>A JSON string representation of the <see cref="Group"/> object.</returns>
        public static string ToJson(this Group group, FaceRecognitionModel recognitionModel)
        {
            var propertiesJson = group.PropertiesToJson();

            var wrappedObject = new { name = group.Name, userData = propertiesJson, recognitionModel = recognitionModel.ToString() };
            return JsonSerializer.Serialize(wrappedObject);
        }

        /// <summary>
        /// Converts the properties of the <see cref="Group"/> object to a JSON string.
        /// </summary>
        /// <param name="group">The <see cref="Group"/> object whose properties are to be converted.</param>
        /// <returns>A JSON string representation of the properties of the <see cref="Group"/> object.</returns>
        public static string PropertiesToJson(this Group group)
        {
            var propertiesJson =
                    group.Properties != null ?
                    JsonSerializer.Serialize(group.Properties) : "{}";
            return propertiesJson;
        }
    }
}
