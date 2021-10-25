using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.HttpHandler
{
    public class HttpHandler : IHttpHandler
    {
        private static readonly HttpClient _httpClient = new HttpClient();

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

        public async Task<HttpResponseMessage> SendJsonPutRequestAsync(string url, object body, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : CreateUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, urlWithParameters))
            {
                PopulateRequestMessageHeaders(headers, requestMessage);
                var requestBodyAsJson = JsonConvert.SerializeObject(body);
                requestMessage.Content = new StringContent(requestBodyAsJson, Encoding.UTF8, "application/json");
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
