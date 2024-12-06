using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface for face recognition operations.
/// </summary>
namespace FACEIT.Core.Interfaces
{
    /// <summary>
    /// Interface for face recognition operations.
    /// </summary>
    public interface IFaceRecognizer
    {
        /// <summary>
        /// Recognizes faces in a given image within a specified group.
        /// </summary>
        /// <param name="groupId">The ID of the group to search within.</param>
        /// <param name="faceImageId">The ID of the face image to recognize.</param>
        /// <param name="confidenceThreshold">The confidence threshold for recognition.</param>
        /// <param name="token">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response with a collection of recognized persons.</returns>
        Task<Response<IEnumerable<RecognizedPerson>>> RecognizeAsync(string groupId, string faceImageId, float confidenceThreshold, CancellationToken token = default);

        /// <summary>
        /// Detects faces in the provided image data.
        /// </summary>
        /// <param name="imageData">The stream containing the image data.</param>
        /// <param name="timeToLive">The time to live for the detection result.</param>
        /// <param name="token">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a response with the detection result as a string.</returns>
        Task<Response<string>> DetectAsync(Stream imageData, int timeToLive, CancellationToken token = default);
    }
}
