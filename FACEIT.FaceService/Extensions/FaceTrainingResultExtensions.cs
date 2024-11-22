using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Azure.AI.Vision.Face
{
    internal static class FaceTrainingResultExtensions
    {
        internal static GroupTrainingData ToGroupTrainingData(this FaceTrainingResult source, string groupId)
        {
            return new GroupTrainingData()
            {
                GroupId = groupId,
                Status = source.Status.ToString(),
                CreatedDateTime=source.CreatedDateTime,
                LastActionDateTime =source.LastActionDateTime,
                LastSuccessfulTrainingDateTime = source.LastSuccessfulTrainingDateTime,
                ErrorMessage =source.Message
            };
        }
    }
}
