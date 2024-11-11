using FACEIT.Core.Entities;
using FACEIT.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.FaceService.Implementations
{
    public class FacesManager : IFacesManager
    {
        private readonly HttpClient _httpClient;

        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly ILogger<FacesManager> _logger;

        private string _recognitionModel = "recognition_04";
        public FacesManager(HttpClient httpClient, string endpoint, string apiKey,ILogger<FacesManager> logger)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
            _apiKey = apiKey;
            _logger = logger;
        }

        public async Task<Response<Group>> CreateGroupAsync(string groupId, string name, string? groupData = null, CancellationToken token = default)
        {
            var response = new Response<Group>();

            if (string.IsNullOrWhiteSpace(groupId))
            {
                response.Success = false;
                response.Message = "Group ID cannot be null or empty.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                response.Success = false;
                response.Message = "Group name cannot be null or empty.";
                return response;
            }

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}";

            var group = new Group() { Id = groupId, Name = name, Data = groupData };

            using var requestContent = new ByteArrayContent(Encoding.UTF8.GetBytes(group.ToJson(_recognitionModel)));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            requestContent.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);

            try
            {
                var faceResponse = await _httpClient.PutAsync(serviceUrl, requestContent, token);

                if (faceResponse.IsSuccessStatusCode)
                {
                    response.Data = group;
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.ReasonPhrase;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating group {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public Task<Response<Group>> GetGroupAsync(string groupId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<Group>>> GetGroupsAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RemoveGroupAsync(string groupId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
    {
    }
}
