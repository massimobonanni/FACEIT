using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Configurations
{
    internal class FacesManagerConfiguration
    {
        const string ConfigRootName = "FacesManager";

        public string Key { get; set; }

        public string Endpoint { get; set; }


        public static FacesManagerConfiguration Load(IConfiguration config)
        {
            var retVal = new FacesManagerConfiguration();
            retVal.Key = config[$"{ConfigRootName}:Key"];
            retVal.Endpoint = config[$"{ConfigRootName}:Endpoint"];
            return retVal;
        }
    }
}
