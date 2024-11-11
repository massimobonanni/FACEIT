using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    internal static class GroupExtensions
    {
        public static string ToJson(this Group group, string recognitionModel)
        {
            return $@"{{ ""name"": ""{group.Name}"", ""userData"": ""{group.Data}"", ""recognitionModel"": ""{recognitionModel}"" }}";
        }
    }
}
