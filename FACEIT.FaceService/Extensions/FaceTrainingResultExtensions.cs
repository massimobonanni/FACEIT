using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Azure.AI.Vision.Face
{
    /// <summary>
    /// Provides extension methods for converting <see cref="FaceTrainingResult"/> to other types.
    /// </summary>
    internal static class FaceTrainingResultExtensions
    {
        /// <summary>
        /// Converts a <see cref="FaceTrainingResult"/> to a <see cref="GroupTrainingData"/>.
        /// </summary>
        /// <param name="source">The source <see cref="FaceTrainingResult"/> to convert.</param>
        /// <param name="groupId">The unique identifier for the group.</param>
        /// <returns>A <see cref="GroupTrainingData"/> object populated with data from the <see cref="FaceTrainingResult"/>.</returns>
        internal static GroupTrainingData ToGroupTrainingData(this FaceTrainingResult source, string groupId)
        {
            return new GroupTrainingData()
            {
                GroupId = groupId,
                Status = source.Status.ToString(),
                CreatedDateTime = source.CreatedDateTime,
                LastActionDateTime = source.LastActionDateTime,
                LastSuccessfulTrainingDateTime = source.LastSuccessfulTrainingDateTime,
                ErrorMessage = source.Message
            };
        }
    }
}
