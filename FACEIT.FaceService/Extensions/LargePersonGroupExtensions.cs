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
    /// <summary>
    /// Provides extension methods for the <see cref="LargePersonGroup"/> class.
    /// </summary>
    internal static class LargePersonGroupExtensions
    {
        /// <summary>
        /// Converts a <see cref="LargePersonGroup"/> instance to a <see cref="Group"/> instance.
        /// </summary>
        /// <param name="source">The <see cref="LargePersonGroup"/> instance to convert.</param>
        /// <returns>A <see cref="Group"/> instance with properties copied from the <see cref="LargePersonGroup"/> instance.</returns>
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
