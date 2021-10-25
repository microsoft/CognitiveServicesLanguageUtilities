using CustomTextAnalytics.MiniSDK.RestClient.Enums;
using CustomTextAnalytics.MiniSDK.RestClient.Models.AnalyzeApi;
using CustomTextAnalytics.MiniSDK.RestClient.Models.GetJobResultApi;
using CustomTextAnalytics.MiniSDK.RestClient.Pipeline;
using CustomTextAnalytics.MiniSDK.RestClient.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomTextAnalytics.MiniSDK.RestClient
{
    public class CustomTextAnalyticsRestClient
    {
        private static HttpHandler _httpHandler = new HttpHandler();
        private string _endpointUrl;
        private string _serviceKey;
        private readonly string _customTextAnalyticsBaseUrl = "text/analytics/v3.2-preview.2";

        public CustomTextAnalyticsRestClient(string endpointUrl, string serviceKey)
        {
            _endpointUrl = endpointUrl;
            _serviceKey = serviceKey;
        }

        public async Task<string> StartAnalyzeCustomEntitiesAsync(string documentText, string projectName, string deploymentName)
        {
            // prepare api data
            var url = string.Format("{0}/{1}/analyze", _endpointUrl, _customTextAnalyticsBaseUrl);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            var body = new AnalyzeApiRequestBody(documentText, projectName, deploymentName);

            // make network call
            var response = await _httpHandler.SendHttpRequestAsync<AnalyzeApiRequestBody>(method: HttpRequestMethod.POST, url: url, urlParameters: null, headers: headers, body);

            // extract job id from header
            var operationLocationHeader = HttpHandler.GetHeaderValue(response, "operation-location");
            var jobId = Helpers.ExtractJobIdFromLocationHeader(operationLocationHeader);

            return jobId;
        }

        public async Task<GetJobResultApiResponse> GetAnalyzeJobInfo(string jobId)
        {
            // prepare api data
            var url = string.Format("{0}/{1}/analyze/jobs/{2}", _endpointUrl, _customTextAnalyticsBaseUrl, jobId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _serviceKey
            };

            // make network call
            var response = await _httpHandler.SendHttpRequestAsync<object>(method: HttpRequestMethod.GET, url: url, urlParameters: null, headers: headers);

            // parse result
            var result = JsonConvert.DeserializeObject<GetJobResultApiResponse>(await response.Content.ReadAsStringAsync());

            return result;
        }
    }
}
