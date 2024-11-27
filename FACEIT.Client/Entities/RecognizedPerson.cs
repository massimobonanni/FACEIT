using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Entities
{
    internal class RecognizedPerson:Core.Entities.RecognizedPerson
    {
        public Entities.Person Person { get; set; }

    }
}
