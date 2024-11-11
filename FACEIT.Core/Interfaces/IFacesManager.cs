using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FACEIT.Core.Interfaces
{
    public interface IFacesManager
    {
        Task<Response<Entities.Group>> CreateGroupAsync(string groupId, string name, string? groupData = null, CancellationToken token = default);
        Task<Response<Entities.Group>> GetGroupAsync(string groupId, CancellationToken token = default);
        Task<Response<IEnumerable<Entities.Group>>> GetGroupsAsync(CancellationToken token = default);
        Task<Response> RemoveGroupAsync(string groupId, CancellationToken token = default);
    }
}
