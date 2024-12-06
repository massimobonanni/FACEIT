using FACEIT.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FACEIT.Core.Interfaces
{
    /// <summary>
    /// Interface for managing persons within a group.
    /// </summary>
    public interface IPersonsManager
    {
        /// <summary>
        /// Creates a new person in the specified group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="personName">The name of the person.</param>
        /// <param name="properties">Optional properties of the person.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the created person.</returns>
        Task<Response<Person>> CreatePersonAsync(string groupId, string personName, IDictionary<string, string>? properties = null, CancellationToken token = default);

        /// <summary>
        /// Adds an image to the specified person.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="personId">The ID of the person.</param>
        /// <param name="imageData">The image data stream.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the image ID.</returns>
        Task<Response<string>> AddImageToPersonAsync(string groupId, string personId, Stream imageData, CancellationToken token = default);

        /// <summary>
        /// Removes the specified person from the group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="personId">The ID of the person.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<Response> RemovePersonAsync(string groupId, string personId, CancellationToken token = default);

        /// <summary>
        /// Gets all persons in the specified group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the list of persons.</returns>
        Task<Response<IEnumerable<Person>>> GetPersonsByGroupAsync(string groupId, CancellationToken token = default);

        /// <summary>
        /// Gets the specified person from the group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="personId">The ID of the person.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the person.</returns>
        Task<Response<Person>> GetPersonAsync(string groupId, string personId, CancellationToken token = default);

        /// <summary>
        /// Removes the specified image from the person.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="personId">The ID of the person.</param>
        /// <param name="persistedImageId">The ID of the image to be removed.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<Response> RemoveImageFromPersonAsync(string groupId, string personId, string persistedImageId, CancellationToken token = default);

        /// <summary>
        /// Updates the specified person in the group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="personId">The ID of the person.</param>
        /// <param name="name">The new name of the person.</param>
        /// <param name="properties">Optional properties of the person.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<Response> UpdatePersonAsync(string groupId, string personId, string name, IDictionary<string, string>? properties = null, CancellationToken token = default);
    }
}