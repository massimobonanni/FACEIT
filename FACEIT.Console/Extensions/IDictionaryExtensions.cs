using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    internal static class IDictionaryExtensions
    {
        internal static string FormatProperties(this IDictionary<string, string>? source)
        {
            if (source == null || !source.Any())
            {
                return string.Empty;
            }

            return string.Join("; ", source.Select(kv => $"{kv.Key}={kv.Value}"));
        }
    }
}
