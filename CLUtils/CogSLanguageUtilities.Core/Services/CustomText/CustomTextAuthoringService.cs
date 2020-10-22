using Microsoft.CogSLanguageUtilities.Definitions.APIs.Helpers.HttpHandler;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Evaluation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.CustomText
{
    class CustomTextAuthoringService : ICustomTextAuthoringService
    {
        private readonly string _customTextKey;
        private readonly string _endpointUrl;
        private readonly string _appId;
        private readonly IHttpHandler _httpHandler;
        private readonly string _customTextAuthoringBaseUrl = "luis/authoring/v4.0-preview/documents";

        public CustomTextAuthoringService(IHttpHandler httpHandler, string customTextKey, string endpointUrl, string appId)
        {
            _customTextKey = customTextKey;
            _endpointUrl = endpointUrl;
            _appId = appId;
            _httpHandler = httpHandler;
            TestConnectionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<CustomTextGetLabeledExamplesResponse> GetLabeledExamples(int skip = 0, int take = Constants.CustomTextExamplesPageSize)
        {
            var requestUrl = string.Format("{0}/{1}/apps/{2}/examples", _endpointUrl, _customTextAuthoringBaseUrl, _appId);
            var parameters = new Dictionary<string, string>
            {
                ["enableNestedChildren"] = "true",
                ["skip"] = skip.ToString(),
                ["take"] = take.ToString()
            };
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, parameters);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var examples = JsonConvert.DeserializeObject<CustomTextGetLabeledExamplesResponse>(responseString);
                if (!string.IsNullOrEmpty(examples.NextPageLink))
                {
                    var nextPage = await GetLabeledExamples(skip + take, take);
                    examples.Examples.AddRange(nextPage.Examples);
                }
                return examples;
            }
            else
            {
                throw new FetchingExamplesFailedException(response.StatusCode.ToString());
            }
        }

        public async Task<Dictionary<string, string>> GetModelsDictionary()
        {
            // get application models
            var applicationModels = await GetApplicationModels();

            // map models
            var result = new Dictionary<string, string>();
            AddModelToDictionaryRecursively(result, applicationModels.Models);
            return result;
        }

        private void AddModelToDictionaryRecursively(Dictionary<string, string> modelsDictionary, List<CustomTextModel> models)
        {
            models.ForEach(m =>
            {
                modelsDictionary.Add(m.Id, m.Name);
                if (m.Children != null)
                {
                    AddModelToDictionaryRecursively(modelsDictionary, m.Children);
                }
            });
        }

        public async Task<CustomTextGetModelsResponse> GetApplicationModels()
        {
            var requestUrl = string.Format("{0}/{1}/apps/{2}/models", _endpointUrl, _customTextAuthoringBaseUrl, _appId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomTextGetModelsResponse>(responseString);
            }
            else
            {
                throw new FetchingExamplesFailedException(response.StatusCode.ToString());
            }
        }

        private async Task TestConnectionAsync()
        {
            var requestUrl = string.Format("{0}/{1}/apps/{2}", _endpointUrl, _customTextAuthoringBaseUrl, _appId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, null);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new FetchingExamplesFailedException(response.StatusCode.ToString());
            }
        }
    }
}
