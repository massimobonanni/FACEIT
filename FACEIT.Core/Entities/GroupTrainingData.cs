using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    /// <summary>
    /// Represents the training data for a group.
    /// </summary>
    public class GroupTrainingData
    {
        /// <summary>
        /// Gets or sets the unique identifier for the group.
        /// </summary>
        public required string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the status of the group training.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the group was created.
        /// </summary>
        public DateTimeOffset? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last action performed on the group.
        /// </summary>
        public DateTimeOffset? LastActionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last successful training of the group.
        /// </summary>
        public DateTimeOffset? LastSuccessfulTrainingDateTime { get; set; }

        /// <summary>
        /// Gets or sets the error message if the training failed.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
