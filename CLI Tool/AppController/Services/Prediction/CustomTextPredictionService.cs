using CustomTextCliUtils.AppController.Models.HttpModels.Prediction;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.Services.Prediction
{
    class CustomTextPredictionService : IPredictionService
    {
        private readonly string _customTextKey;
        private readonly string _endpointUrl;
        private readonly string _appId;
        private static HttpClient httpClient = new HttpClient();
        public CustomTextPredictionService(string customTextKey, string endpointUrl, string appId) {
            _customTextKey = customTextKey;
            _endpointUrl = endpointUrl;
            _appId = appId; 
        }
        public async Task<CustomTextPredictionResponse> PredictAsync(string query)
        {
            // send prediciotn request
            var operationId = await SendPredictionRequestAsync(query);
            // get result when status is 'success'
            var finished = false;
            while (!finished) {
                var pingResult = await PingStatusAsync(operationId);
                if (pingResult == "succeeded") {
                    finished = true;
                }
            }
            // get result
            var prediction = await GetResultAsync(operationId);
            return prediction;
        }

        private async Task<string> SendPredictionRequestAsync(string query) {
            /*
             * request form
             * https://nayergroup.cognitiveservices.azure.com/luis/prediction/v4.0-preview/documents/apps//5c0df28e-335a-4ff7-8580-91172fd57422/slots/production/predictText?log=true&%24expand=classifier%2Cextractor
            */
            // url
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/predictText?log=true&%24expand=classifier%2Cextractor", _endpointUrl, _appId);
            // body
            var body = new Dictionary<string, string>
                {
                    { "query", query}
                };
            var bodyAsJson = JsonConvert.SerializeObject(body);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _customTextKey);
                requestMessage.Content = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                var responseContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                return responseContent["operationId"];
            }
        }

        private async Task<string> PingStatusAsync(string operationId) {
            /*
             https://nayergroup.cognitiveservices.azure.com/luis/prediction/v4.0-preview/documents/apps/5c0df28e-335a-4ff7-8580-91172fd57422/slots/production/operations/64017d1d-7728-411d-871a-8d4b2a4779d8_637337376000000000/predictText
             */
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/operations/{2}/predictText/status", _endpointUrl, _appId, operationId);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _customTextKey);
                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                var responseContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                return responseContent["status"];
            }
        }

        private async Task<CustomTextPredictionResponse> GetResultAsync(string operationId)
        {
            /*
             https://nayergroup.cognitiveservices.azure.com/luis/prediction/v4.0-preview/documents/apps/5c0df28e-335a-4ff7-8580-91172fd57422/slots/production/operations/64017d1d-7728-411d-871a-8d4b2a4779d8_637337376000000000/predictText
             */
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/operations/{2}/predictText", _endpointUrl, _appId, operationId);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _customTextKey);
                HttpResponseMessage response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<CustomTextPredictionResponse>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
