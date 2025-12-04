using FACEIT.Core.Interfaces;
using FACEIT.FaceService.Implementations;
using Microsoft.Extensions.Logging;

namespace FACEIT.Console.Services
{
    /// <summary>
    /// Factory class for creating face service instances.
    /// </summary>
    internal class FaceServiceFactory : IFaceServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly HttpClient _httpClient;

        public FaceServiceFactory(ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            _loggerFactory = loggerFactory;
            _httpClient = httpClient;
        }

        public IGroupsManager CreateGroupsManager(string? endpoint, string? apiKey)
        {
            var logger = _loggerFactory.CreateLogger<FacesManager>();
            return new FacesManager(_httpClient, endpoint ?? string.Empty, apiKey ?? string.Empty, logger);
        }

        public IPersonsManager CreatePersonsManager(string? endpoint, string? apiKey)
        {
            var logger = _loggerFactory.CreateLogger<FacesManager>();
            return new FacesManager(_httpClient, endpoint ?? string.Empty, apiKey ?? string.Empty, logger);
        }

        public IFaceRecognizer CreateFaceRecognizer(string? endpoint, string? apiKey)
        {
            var logger = _loggerFactory.CreateLogger<FacesManager>();
            return new FacesManager(_httpClient, endpoint ?? string.Empty, apiKey ?? string.Empty, logger);
        }
    }
}
