using FACEIT.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FACEIT.Core.Interfaces
{
    /// <summary>
    /// Interface for managing groups.
    /// </summary>
    public interface IGroupsManager
    {
        /// <summary>
        /// Creates a new group asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="properties">Optional properties of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the created group.</returns>
        Task<Response<Group>> CreateGroupAsync(string groupId, string name, IDictionary<string, string>? properties = null, CancellationToken token = default);

        /// <summary>
        /// Gets a group by its ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the group.</returns>
        Task<Response<Group>> GetGroupAsync(string groupId, CancellationToken token = default);

        /// <summary>
        /// Gets all groups asynchronously.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the list of groups.</returns>
        Task<Response<IEnumerable<Group>>> GetGroupsAsync(CancellationToken token = default);

        /// <summary>
        /// Removes a group by its ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<Response> RemoveGroupAsync(string groupId, CancellationToken token = default);

        /// <summary>
        /// Trains a group by its ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the group training data.</returns>
        Task<Response<GroupTrainingData>> TrainGroupAsync(string groupId, CancellationToken token = default);

        /// <summary>
        /// Gets the training status of a group by its ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response with the group training data.</returns>
        Task<Response<GroupTrainingData>> GetTrainingStatusAsync(string groupId, CancellationToken token = default);

        /// <summary>
        /// Updates a group by its ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="name">The new name of the group.</param>
        /// <param name="properties">Optional new properties of the group.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<Response> UpdateGroupAsync(string groupId, string name, IDictionary<string, string>? properties = null, CancellationToken token = default);

        /// <summary>
        /// Clears all groups asynchronously.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>
        Task<Response> ClearAllAsync(CancellationToken token = default);
    }
}
