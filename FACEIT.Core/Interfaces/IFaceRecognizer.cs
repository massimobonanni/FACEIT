using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Interfaces
{
    public interface IFaceRecognizer
    {
        Task<Response<IEnumerable<RecognizedPerson>>> RecognizeAsync(string groupId, string faceImageId, float confidenceThreshold, CancellationToken token = default);

        Task<Response<string>> DetectAsync(Stream imageData, int timeToLive, CancellationToken token = default);
    }
}
