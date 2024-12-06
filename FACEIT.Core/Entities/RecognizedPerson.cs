using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    /// <summary>
    /// Represents a person recognized by the system.
    /// </summary>
    public class RecognizedPerson
    {
        /// <summary>
        /// Gets or sets the unique identifier of the recognized person.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the confidence level of the recognition.
        /// </summary>
        public float Confidence { get; set; }
    }
}
