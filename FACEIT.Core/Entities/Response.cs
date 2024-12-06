using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    /// <summary>
    /// Represents a response with a success status and an optional message.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Gets or sets a value indicating whether the response is successful.
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Gets or sets an optional message associated with the response.
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// Represents a response with a success status, an optional message, and associated data.
    /// </summary>
    /// <typeparam name="T">The type of the data associated with the response.</typeparam>
    public class Response<T> : Response
    {
        /// <summary>
        /// Gets or sets the data associated with the response.
        /// </summary>
        public T? Data { get; set; }
    }
}
