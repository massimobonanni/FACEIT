using FACEIT.Client.Configurations;
using FACEIT.Core.Interfaces;
using FACEIT.FaceService.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceProviderExtensions
    {
        public static IServiceCollection UsePersonsManager(this IServiceCollection services)
        {
            services.AddSingleton<IPersonsManager>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<FacesManager>();
                var config = FacesManagerConfiguration.Load(sp.GetRequiredService<IConfiguration>());
                if (config.UseIdentityAuthorization)
                {
                    return new FacesManager(httpClient, config.Endpoint, config.ClientId, config.TenantId, config.ClientSecret, logger);
                }
                return new FacesManager(httpClient, config.Endpoint, config.Key, logger);
            });
            return services;
        }

        public static IServiceCollection UseGroupsManager(this IServiceCollection services)
        {
            services.AddSingleton<IGroupsManager>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<FacesManager>();
                var config = FacesManagerConfiguration.Load(sp.GetRequiredService<IConfiguration>());
                if (config.UseIdentityAuthorization)
                {
                    return new FacesManager(httpClient, config.Endpoint, config.ClientId, config.TenantId, config.ClientSecret, logger);
                }
                return new FacesManager(httpClient, config.Endpoint, config.Key, logger);
            });
            return services;
        }

        public static IServiceCollection UseFaceRecognizer(this IServiceCollection services)
        {
            services.AddSingleton<IFaceRecognizer>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<FacesManager>();
                var config = FacesManagerConfiguration.Load(sp.GetRequiredService<IConfiguration>());
                if (config.UseIdentityAuthorization)
                {
                    return new FacesManager(httpClient, config.Endpoint, config.ClientId, config.TenantId, config.ClientSecret, logger);
                }
                return new FacesManager(httpClient, config.Endpoint, config.Key, logger);
            });
            return services;
        }
    }
}
