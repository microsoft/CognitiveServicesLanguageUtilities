using System;
using System.Collections.Generic;
using System.Net.Http;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler
{
    public interface IHttpHandler
    {
        public HttpResponseMessage SendGetRequest(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters);

        public HttpResponseMessage SendJsonPostRequest(string url, Object body, Dictionary<string, string> headers, Dictionary<string, string> parameters);
    }
}
