using FACEIT.Core.Entities;
using FACEIT.Core.Interfaces;
using FACEIT.FaceService.Utilities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Group = FACEIT.Core.Entities.Group;

namespace FACEIT.FaceService.Implementations
{
    ///  https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/quickstarts-sdk/identity-client-library?tabs=windows%2Cvisual-studio&pivots=programming-language-rest-api

    public class FacesManager : IGroupsManager, IPersonsManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly ILogger<FacesManager> _logger;
        private string _recognitionModel = "recognition_04"; // https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/specify-recognition-model
        private string _detectionModel = "detection_03"; // https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/specify-detection-model

        public FacesManager(HttpClient httpClient, string endpoint, string apiKey, ILogger<FacesManager> logger)
        {
            _httpClient = httpClient;
            _endpoint = endpoint.TrimEnd('/');
            _apiKey = apiKey;
            _logger = logger;
        }

        private async Task<Response<T>> SendRequestAsync<T>(HttpMethod method, string url, HttpContent? content = null, CancellationToken token = default)
        {
            var response = new Response<T>();

            using var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", _apiKey);
            if (content != null)
                request.Content = content;

            try
            {
                var httpResponse = await _httpClient.SendAsync(request, token);

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (typeof(T) == typeof(string))
                    {
                        response.Data = (T)(object)(await httpResponse.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        var responseData = await httpResponse.Content.ReadAsStringAsync();
                        response.Data = JsonSerializer.Deserialize<T>(responseData);
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = httpResponse.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to {url}", url);
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        #region [ IGroupsManager interface ]
        public async Task<Response<Group>> CreateGroupAsync(string groupId, string name, IDictionary<string, string>? properties = null, CancellationToken token = default)
        {
            var response = new Response<Group>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidateGroupName(name, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}";
            var group = new Group() { Id = groupId, Name = name, Properties = properties };
            var groupJson = group.ToJson(_recognitionModel);
            using var requestContent = new StringContent(groupJson, Encoding.UTF8, "application/json");

            return await SendRequestAsync<Group>(HttpMethod.Put, serviceUrl, requestContent, token);
        }

        public async Task<Response> RemoveGroupAsync(string groupId, CancellationToken token = default)
        {
            var response = new Response();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}";

            return await SendRequestAsync<string>(HttpMethod.Delete, serviceUrl, null, token);
        }

        public async Task<Response<Group>> GetGroupAsync(string groupId, CancellationToken token = default)
        {
            var response = new Response<Group>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}";

            var httpResponse = await SendRequestAsync<FaceService.Entities.Group>(HttpMethod.Get, serviceUrl, null, token);
            if (httpResponse.Success && httpResponse.Data != null)
            {
                response.Data = httpResponse.Data.ToCoreGroup();
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message ?? "Group not found";
            }

            return response;
        }

        public async Task<Response<IEnumerable<Group>>> GetGroupsAsync(CancellationToken token = default)
        {
            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups";

            var httpResponse = await SendRequestAsync<IEnumerable<FaceService.Entities.Group>>(HttpMethod.Get, serviceUrl, null, token);
            var response = new Response<IEnumerable<Group>>();

            if (httpResponse.Success && httpResponse.Data != null)
            {
                response.Data = httpResponse.Data.Select(g => g.ToCoreGroup()).ToList();
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message;
            }

            return response;
        }

        public async Task<Response> TrainGroupAsync(string groupId, CancellationToken token = default)
        {
            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}/train";

            return await SendRequestAsync<string>(HttpMethod.Post, serviceUrl, null, token);
        }

        public async Task<Response<string>> GetTrainingStatusAsync(string groupId, CancellationToken token = default)
        {
            var response = new Response<string>();

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}/training";

            var httpResponse = await SendRequestAsync<string>(HttpMethod.Get, serviceUrl, null, token);

            if (httpResponse.Success && httpResponse.Data != null)
            {
                using var document = JsonDocument.Parse(httpResponse.Data);
                if (document.RootElement.TryGetProperty("status", out var status))
                {
                    response.Data = status.GetString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Generic error during status retrieving";
                }
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message;
            }

            return response;
        }
        #endregion [ IGroupsManager interface ]

        #region [ IPersonsManager interface ]
        public async Task<Response<string>> AddImageToPersonAsync(string groupId, string personId, Stream imageData, CancellationToken token = default)
        {
            var response = new Response<string>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidatePersonId(personId, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}/persons/{personId}/persistedfaces?detectionModel={_detectionModel}";

            using var requestContent = new StreamContent(imageData);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var httpResponse = await SendRequestAsync<string>(HttpMethod.Post, serviceUrl, requestContent, token);

            if (httpResponse.Success && httpResponse.Data != null)
            {
                using var document = JsonDocument.Parse(httpResponse.Data);
                if (document.RootElement.TryGetProperty("persistedFaceId", out var persistedFaceIdElement))
                {
                    response.Data = persistedFaceIdElement.GetString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Generic error during image addition";
                }
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message;
            }

            return response;
        }

        public async Task<Response<Person>> CreatePersonAsync(string groupId, string personName, IDictionary<string, string>? properties = null, CancellationToken token = default)
        {
            var response = new Response<Person>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidatePersonName(personName, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}/persons";
            var person = new Person() { Name = personName, Properties = properties };
            var personJson = person.ToJson();
            using var requestContent = new StringContent(personJson, Encoding.UTF8, "application/json");

            var httpResponse = await SendRequestAsync<string>(HttpMethod.Post, serviceUrl, requestContent, token);

            if (httpResponse.Success && httpResponse.Data != null)
            {
                using var document = JsonDocument.Parse(httpResponse.Data);
                if (document.RootElement.TryGetProperty("personId", out var personIdElement))
                {
                    person.Id = personIdElement.GetString();
                    response.Data = person;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Generic error during person creation";
                }
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message;
            }

            return response;
        }

        public async Task<Response<Person>> GetPersonAsync(string groupId, string personId, CancellationToken token = default)
        {
            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}/persons/{personId}";

            var httpResponse = await SendRequestAsync<FaceService.Entities.Person>(HttpMethod.Get, serviceUrl, null, token);
            var response = new Response<Person>();

            if (httpResponse.Success && httpResponse.Data != null)
            {
                response.Data = httpResponse.Data.ToCorePerson();
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message;
            }

            return response;
        }

        public async Task<Response<IEnumerable<Person>>> GetPersonsByGroupAsync(string groupId, CancellationToken token = default)
        {
            var serviceUrl = $"{_endpoint}/face/v1.0/persongroups/{groupId}/persons";

            var httpResponse = await SendRequestAsync<IEnumerable<FaceService.Entities.Person>>(HttpMethod.Get, serviceUrl, null, token);
            var response = new Response<IEnumerable<Person>>();

            if (httpResponse.Success && httpResponse.Data != null)
            {
                response.Data = httpResponse.Data.Select(g => g.ToCorePerson()).ToList();
            }
            else
            {
                response.Success = false;
                response.Message = httpResponse.Message;
            }

            return response;
        }

        public Task<Response> RemovePersonAsync(string groupId, string personId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
        #endregion [ IPersonsManager interface ]
    }
}
