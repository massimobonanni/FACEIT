using FACEIT.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FACEIT.Core.Interfaces
{
    public interface IGroupsManager
    {
        Task<Response<Group>> CreateGroupAsync(string groupId, string name, IDictionary<string, string>? properties = null, CancellationToken token = default);
        Task<Response<Group>> GetGroupAsync(string groupId, CancellationToken token = default);
        Task<Response<IEnumerable<Group>>> GetGroupsAsync(CancellationToken token = default);
        Task<Response> RemoveGroupAsync(string groupId, CancellationToken token = default);
        Task<Response> TrainGroupAsync(string groupId, CancellationToken token = default);
        Task<Response<string>> GetTrainingStatusAsync(string groupId, CancellationToken token = default);
    }
}
