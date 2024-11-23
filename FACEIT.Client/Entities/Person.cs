using FACEIT.Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Entities
{
    internal class Person : Core.Entities.Person
    {
        public bool IsNew { get; set; } = false;

        public int NumberOfImages { get => this.PersistedFaceIds == null ? 0 : this.PersistedFaceIds.Count(); }

        public bool Enabled
        {
            get
            {
                var enabled = DictionaryUtility.GetProperty(this.Properties, "enabled");
                return bool.TryParse(enabled, out bool result) && result;
            }
            set => this.Properties = DictionaryUtility.AddProperty(this.Properties, "enabled", value.ToString());
        }
    }
}
