using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    public class GroupTrainingData
    {
        public required string GroupId { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? LastActionDateTime { get; set; }
        public DateTimeOffset? LastSuccessfulTrainingDateTime { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
