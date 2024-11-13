using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    public class Person
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public IDictionary<string, string>? Properties { get; set; } = new Dictionary<string, string>();

        public IEnumerable<string>? PersistedFaceIds { get; set; }

    }
}
