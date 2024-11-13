using FACEIT.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FACEIT.Core.Interfaces
{
    public interface IPersonsManager
    {
        Task<Response<Person>> CreatePersonAsync(string groupId, string personName, IDictionary<string, string>? properties = null, CancellationToken token = default);

        Task<Response<string>> AddImageToPersonAsync(string groupId, string personId, Stream imageData, CancellationToken token = default);

        Task<Response> RemovePersonAsync(string groupId, string personId, CancellationToken token = default);

        Task<Response<IEnumerable<Person>>> GetPersonsByGroupAsync(string groupId, CancellationToken token = default);

        Task<Response<Person>> GetPersonAsync(string groupId, string personId, CancellationToken token = default);
    }
}