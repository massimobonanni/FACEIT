using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Utilities
{
    internal static class DictionaryUtility
    {
        public static string? GetProperty(IDictionary<string, string> source, string propertyName)
        {
            if (source != null && source.TryGetValue(propertyName, out var value))
            {
                return value;
            }
            return null;
        }

        public static IDictionary<string, string> AddProperty(IDictionary<string, string> source, string key, string value)
        {
            if (source == null)
            {
                source = new Dictionary<string, string>();
            }
            source[key] = value;
            return source;
        }
    }
}
