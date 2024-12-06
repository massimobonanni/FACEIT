using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains the core entities for the FACEIT application.
/// </summary>
namespace FACEIT.Core.Entities
{
    /// <summary>
    /// Represents a group entity with an ID, name, and a collection of properties.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a collection of properties associated with the group.
        /// </summary>
        public IDictionary<string, string>? Properties { get; set; } = new Dictionary<string, string>();
    }
}
