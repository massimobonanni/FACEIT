using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Entities
{
    internal class Person:Core.Entities.Person
    {
        public bool IsNew { get; set; } = false;

        public int NumberOfImages { get => this.PersistedFaceIds == null ? 0 : this.PersistedFaceIds.Count(); }
    }
}
