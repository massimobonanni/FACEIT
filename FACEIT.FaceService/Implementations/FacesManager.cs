using Azure;
using Azure.AI.Vision.Face;
using FACEIT.Core.Entities;
using FACEIT.Core.Interfaces;
using FACEIT.FaceService.Entities;
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
        private FaceRecognitionModel _recognitionModel = FaceRecognitionModel.Recognition04; // https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/specify-recognition-model
        private FaceDetectionModel _detectionModel = FaceDetectionModel.Detection03;// https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/specify-detection-model

        public FacesManager(HttpClient httpClient, string endpoint, string apiKey, ILogger<FacesManager> logger)
        {
            _httpClient = httpClient;
            _endpoint = endpoint.TrimEnd('/');
            _apiKey = apiKey;
            _logger = logger;
        }

        private LargePersonGroupClient CreateLargePersonGroupClient(string groupId)
        {
            return new FaceAdministrationClient(new Uri(_endpoint), new AzureKeyCredential(_apiKey)).GetLargePersonGroupClient(groupId);
        }

        private async Task<Core.Entities.Response<T>> SendRequestAsync<T>(HttpMethod method, string url, HttpContent? content = null, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<T>();

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
        public async Task<Core.Entities.Response<Group>> CreateGroupAsync(string groupId, string name, IDictionary<string, string>? properties = null, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<Group>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                            !ValidationUtility.ValidateGroupName(name, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            var group = new Group() { Id = groupId, Name = name, Properties = properties };

            try
            {
                LargePersonGroupClient client = CreateLargePersonGroupClient(groupId);
                var faceResponse = await client.CreateAsync(group.Name, group.PropertiesToJson(), _recognitionModel, token);

                if (!faceResponse.IsError)
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
                _logger.LogError(ex, "Unexpected error creating group with ID {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response> RemoveGroupAsync(string groupId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                LargePersonGroupClient client = CreateLargePersonGroupClient(groupId);

                var faceResponse = await client.DeleteAsync();

                if (faceResponse.IsError)
                {
                    response.Success = false;
                    response.Message = faceResponse.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error removing group with ID {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<Core.Entities.Response<Group>> GetGroupAsync(string groupId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<Group>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                var client = CreateLargePersonGroupClient(groupId);

                var faceResponse = await client.GetLargePersonGroupAsync(cancellationToken: token);

                if (!faceResponse.GetRawResponse().IsError)
                {
                    response.Data = faceResponse.Value.ToCoreGroup();
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting group with ID {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response<IEnumerable<Group>>> GetGroupsAsync(CancellationToken token = default)
        {
            var serviceUrl = $"{_endpoint}/face/v1.0/largepersongroups";

            var httpResponse = await SendRequestAsync<IEnumerable<FaceService.Entities.Group>>(HttpMethod.Get, serviceUrl, null, token);
            var response = new Core.Entities.Response<IEnumerable<Group>>();

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

        //public async Task<Core.Entities.Response<IEnumerable<Group>>> GetGroupsAsync(CancellationToken token = default)
        //{
        //    var response = new Core.Entities.Response<IEnumerable<Group>>();

        //    var client = CreateLargePersonGroupClient(null);

        //    var faceResponse = await client.GetLargePersonGroupsAsync(cancellationToken: token);

        //    if (!faceResponse.GetRawResponse().IsError)
        //    {
        //        response.Data = faceResponse.Value.Select(g => g.ToCoreGroup()).ToList();
        //    }
        //    else
        //    {
        //        response.Success = false;
        //        response.Message = faceResponse.GetRawResponse().ReasonPhrase;
        //    }

        //    return response;
        //}

        public async Task<Core.Entities.Response> TrainGroupAsync(string groupId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response();

            try
            {
                var client = CreateLargePersonGroupClient(groupId);

                var faceResponse = await client.TrainAsync(WaitUntil.Started);

                if (faceResponse.GetRawResponse().IsError)
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error training group with ID {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response<string>> GetTrainingStatusAsync(string groupId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<string>();

            try
            {
                var client = CreateLargePersonGroupClient(groupId);

                var faceResponse = await client.GetTrainingStatusAsync();

                if (!faceResponse.GetRawResponse().IsError)
                {
                    response.Data = faceResponse.Value.Status.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting training status for group with ID {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion [ IGroupsManager interface ]

        #region [ IPersonsManager interface ]
        public async Task<Core.Entities.Response<string>> AddImageToPersonAsync(string groupId, string personId, Stream imageData, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<string>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidatePersonId(personId, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                var client = CreateLargePersonGroupClient(groupId);

                var binaryData = BinaryData.FromStream(imageData);

                var faceResponse = await client.AddFaceAsync(new Guid(personId), binaryData, null, _detectionModel, null, token);

                if (!faceResponse.GetRawResponse().IsError)
                {
                    response.Data = faceResponse.Value.PersistedFaceId.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding image to person ID {personId} on group {groupId}", personId, groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response<Core.Entities.Person>> CreatePersonAsync(string groupId, string personName, IDictionary<string, string>? properties = null, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<Core.Entities.Person>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidatePersonName(personName, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }
            var person = new Core.Entities.Person() { Name = personName, Properties = properties };

            try
            {
                var client = CreateLargePersonGroupClient(groupId);
                var faceResponse = await client.CreatePersonAsync(person.Name, person.PropertiesToJson(), token);

                if (!faceResponse.GetRawResponse().IsError)
                {
                    person.Id = faceResponse.Value.PersonId.ToString();
                    response.Data = person;
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating person {personName} on group with ID {groupId}", personName, groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response<Core.Entities.Person>> GetPersonAsync(string groupId, string personId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<Core.Entities.Person>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidatePersonId(personId, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                var client = CreateLargePersonGroupClient(groupId);
                var faceResponse = await client.GetPersonAsync(new Guid(personId), token);

                if (!faceResponse.GetRawResponse().IsError)
                {
                    response.Data = faceResponse.Value.ToCorePerson();
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting person with ID {personId} on group with ID {groupId}", personId, groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response<IEnumerable<Core.Entities.Person>>> GetPersonsByGroupAsync(string groupId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response<IEnumerable<Core.Entities.Person>>();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                var client = CreateLargePersonGroupClient(groupId);
                var faceResponse = await client.GetPersonsAsync(cancellationToken: token);

                if (!faceResponse.GetRawResponse().IsError)
                {
                    response.Data = faceResponse.Value.Select(g => g.ToCorePerson()).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = faceResponse.GetRawResponse().ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting persons for group with ID {groupId}", groupId);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Core.Entities.Response> RemovePersonAsync(string groupId, string personId, CancellationToken token = default)
        {
            var response = new Core.Entities.Response();

            if (!ValidationUtility.ValidateGroupId(groupId, out var errorMessage) ||
                !ValidationUtility.ValidatePersonId(personId, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                var client = CreateLargePersonGroupClient(groupId);
                var faceResponse = await client.DeletePersonAsync(new Guid(personId));

                if (faceResponse.IsError)
                {
                    response.Success = false;
                    response.Message = faceResponse.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error removing person with ID {personId} from group with ID {groupId}", personId,groupId);
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
        #endregion [ IPersonsManager interface ]
    }
}
