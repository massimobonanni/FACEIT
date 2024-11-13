﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Core.Entities
{
    public class Group
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public IDictionary<string, string>? Properties { get; set; } = new Dictionary<string, string>();

    }
}
