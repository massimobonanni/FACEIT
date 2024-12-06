using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// Converts an IEnumerable of strings to a dictionary of properties.
    /// Each string in the source should be in the format "key:value".
    /// </summary>
    /// <param name="source">The source IEnumerable of strings.</param>
    /// <returns>
    /// A dictionary where the keys are the parts before the colon and the values are the parts after the colon.
    /// Returns null if the source is null.
    /// </returns>
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
