using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal static class StringExtensions
    {
        public static bool IsValidGroupId(this string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
                return false;

            if (groupId.Any(char.IsWhiteSpace) || groupId.Any(char.IsUpper))
                return false;

            return true;
        }
    }
}
