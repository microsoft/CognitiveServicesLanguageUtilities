using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation
{
    public class CognitiveSearchValidationService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task ValidateCognitiveSearchConfigs(string endpointUrl, string serviceKey)
        {
            try
            {
                await ListIndices(endpointUrl, serviceKey);
            }
            catch (Exception)
            {
                throw new Exception("Cognitive Search Credentials Are Invalid!");
            }
        }

        private async Task ListIndices(string endpointUrl, string serviceKey)
        {
            // "https://[service name].search.windows.net/indexes?api-version=[api-version]"
            var url = string.Format("{0}/indexes", endpointUrl);
            var headers = new Dictionary<string, string>()
            {
                ["api-key"] = serviceKey,
            };
            var parameters = new Dictionary<string, string>()
            {
                ["api-version"] = "2020-06-30",
            };
            var response = await SendGetRequestAsync(url: url, headers: headers, parameters: parameters);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        public async Task<HttpResponseMessage> SendGetRequestAsync(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : CreateUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, urlWithParameters))
            {
                PopulateRequestMessageHeaders(headers, requestMessage);
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
                return response;
            }
        }

        private string CreateUrlWithParameters(string url, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            parameters.ToList().ForEach(p => query[p.Key] = p.Value);
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();
            return url;
        }

        private void PopulateRequestMessageHeaders(Dictionary<string, string> headers, HttpRequestMessage requestMessage)
        {
            foreach (KeyValuePair<string, string> h in headers)
            {
                requestMessage.Headers.Add(h.Key, h.Value);
            }
        }
    }
}
