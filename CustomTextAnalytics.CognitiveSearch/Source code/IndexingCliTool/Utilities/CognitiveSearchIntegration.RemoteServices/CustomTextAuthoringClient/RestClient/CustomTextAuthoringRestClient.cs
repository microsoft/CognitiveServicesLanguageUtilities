using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient
{
    public class CustomTextAuthoringRestClient
    {
        private static HttpHandler _httpHandler = new HttpHandler();
        private ClientHelpers _helpers = new ClientHelpers();
        private string _endpointUrl;
        private string _serviceKey;
        private readonly string _customTextAuthoringBaseUrl = "language/analyze-text";
        private readonly string CustomTextAuthoringApiVersion = "2021-11-01-preview";

        public CustomTextAuthoringRestClient(string endpointUrl, string serviceKey)
        {
            _endpointUrl = endpointUrl;
            _serviceKey = serviceKey;
        }

        public async Task<string> StartProjectExportAsync(string projectName)
        {
            // prepare api data
            var url = string.Format("{0}/{1}/projects/{2}/:export", _endpointUrl, _customTextAuthoringBaseUrl, projectName);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey,
                ["format"] = "json" // only json is supported for now
            };
            var parameters = new Dictionary<string, string>
            {
                ["api-version"] = CustomTextAuthoringApiVersion
            };
            var body = new Dictionary<string, object>
            {
                ["assetsToExport"] = new string[] { "extractors" }
            };

            // make network call
            var response = await _httpHandler.SendJsonPostRequestAsync(url, body, headers, parameters);

            // handle errors if exists
            await _helpers.HandleApiErrorResponse(response);

            // extract job id from header
            var operationLocationHeader = HttpHandler.GetHeaderValue(response, "operation-location");
            var jobId = _helpers.ExtractJobIdFromLocationHeader(operationLocationHeader);

            return jobId;
        }

        public async Task<ProjectFileExportJobStatus> GetProjectExportJobStatus(string projectName, string jobId)
        {
            // prepare api data
            var url = string.Format("{0}/{1}/projects/{2}/export/jobs/{3}", _endpointUrl, _customTextAuthoringBaseUrl, projectName, jobId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };
            var parameters = new Dictionary<string, string>
            {
                ["api-version"] = CustomTextAuthoringApiVersion
            };

            // make network call
            var response = await _httpHandler.SendGetRequestAsync(url, headers, parameters);

            // handle errors if exists
            await _helpers.HandleApiErrorResponse(response);

            // parse result
            var result = JsonHandler.DeserializeObject<ProjectFileExportJobStatus>(await response.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<AnalyzeTextProject> GetProjectExportJobResult(string url)
        {
            // prepare api data
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            // make network call
            var response = await _httpHandler.SendGetRequestAsync(url, headers, default);

            // parse result
            var result = JsonHandler.DeserializeObject<AnalyzeTextProject>(await response.Content.ReadAsStringAsync());

            return result;
        }
    }
}