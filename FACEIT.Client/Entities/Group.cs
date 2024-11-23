using FACEIT.Client.Utilities;
using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Entities
{
    internal class Group : Core.Entities.Group
    {
        public bool IsNew { get; set; } = false;
        public GroupTrainingData TrainingData { get; set; }

        public string Description
        {
            get => DictionaryUtility.GetProperty(this.Properties, "description");
            set => this.Properties = DictionaryUtility.AddProperty(this.Properties, "description", value);
        }



    }
}
