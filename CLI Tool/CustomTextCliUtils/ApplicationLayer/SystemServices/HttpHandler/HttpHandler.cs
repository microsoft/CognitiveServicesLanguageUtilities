using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.SystemServices.HttpHandler
{
    class HttpHandler : IHttpHandler
    {
        private static HttpClient httpClient = new HttpClient();

        public HttpResponseMessage SendGetRequest(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : createUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, urlWithParameters))
            {
                headers?.ToList().ForEach(h => requestMessage.Headers.Add(h.Key, h.Value));
                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                return response;
            }
        }

        public HttpResponseMessage SendJsonPostRequest(string url, Object body, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : createUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, urlWithParameters))
            {
                headers?.ToList().ForEach(h => requestMessage.Headers.Add(h.Key, h.Value));
                var requestBodyAsJson = JsonConvert.SerializeObject(body);
                requestMessage.Content = new StringContent(requestBodyAsJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                return response;
            }
        }

        private string createUrlWithParameters(string url, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            parameters.ToList().ForEach(p => query[p.Key] = p.Value);
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();
            return url;
        }
    }
}
