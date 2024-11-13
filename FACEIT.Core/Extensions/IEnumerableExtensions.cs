using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static IDictionary<string, string>? ToProperties(this IEnumerable<string> source)
        {
            if (source == null)
            {
                return null;
            }
            return source.Select(x => x.Split(':')).ToDictionary(x => x[0], x => x[1]);
        }
    }
}
