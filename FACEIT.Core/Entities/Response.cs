using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    public class Response
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
    }

    public class Response<T> : Response
    {
        public T? Data { get; set; }
    }
}
