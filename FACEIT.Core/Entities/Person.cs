using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    /// <summary>
    /// Represents a person with an ID, name, properties, and persisted face IDs.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the unique identifier for the person.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a collection of properties associated with the person.
        /// </summary>
        public IDictionary<string, string>? Properties { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets a collection of persisted face IDs associated with the person.
        /// </summary>
        public IEnumerable<string>? PersistedFaceIds { get; set; }
    }
}
