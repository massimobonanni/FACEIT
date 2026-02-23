using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Configurations
{
    /// <summary>
    /// Configuration class for the FacesManager service, containing authentication and endpoint settings.
    /// </summary>
    internal class FacesManagerConfiguration
    {
        /// <summary>
        /// The root configuration section name used to load settings from the configuration source.
        /// </summary>
        const string ConfigRootName = "FacesManager";

        /// <summary>
        /// Gets or sets the API key for authentication with the FacesManager service.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the endpoint URL for the FacesManager service.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the Azure AD tenant identifier for identity-based authorization.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client application identifier for identity-based authorization.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret for identity-based authorization.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets a value indicating whether identity-based authorization should be used.
        /// Returns <c>true</c> if TenantId, ClientId, and ClientSecret are all configured; otherwise, <c>false</c>.
        /// </summary>
        public bool UseIdentityAuthorization => !string.IsNullOrEmpty(TenantId) && !string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret);

        /// <summary>
        /// Loads the FacesManager configuration from the specified configuration source.
        /// </summary>
        /// <param name="config">The configuration source containing FacesManager settings.</param>
        /// <returns>A new instance of <see cref="FacesManagerConfiguration"/> populated with values from the configuration source.</returns>
        /// <remarks>
        /// Configuration values are expected to be under the "FacesManager" section with the following keys:
        /// <list type="bullet">
        /// <item><description>Key: API key for service authentication</description></item>
        /// <item><description>Endpoint: Service endpoint URL</description></item>
        /// <item><description>TenantId: Azure AD tenant ID</description></item>
        /// <item><description>ClientId: Application client ID</description></item>
        /// <item><description>ClientSecret: Application client secret</description></item>
        /// </list>
        /// </remarks>
        public static FacesManagerConfiguration Load(IConfiguration config)
        {
            var retVal = new FacesManagerConfiguration
            {
                Key = config[$"{ConfigRootName}:Key"],
                Endpoint = config[$"{ConfigRootName}:Endpoint"],
                TenantId = config[$"{ConfigRootName}:TenantId"],
                ClientId = config[$"{ConfigRootName}:ClientId"],
                ClientSecret = config[$"{ConfigRootName}:ClientSecret"]
            };

            return retVal;
        }
    }
}
