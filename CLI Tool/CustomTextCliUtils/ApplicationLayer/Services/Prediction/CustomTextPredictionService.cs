using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextErrorResponse;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public class CustomTextPredictionService : IPredictionService
    {
        private readonly string _customTextKey;
        private readonly string _endpointUrl;
        private readonly string _appId;
        private static IHttpHandler _httpHandler;

        public CustomTextPredictionService(IHttpHandler httpHandler, string customTextKey, string endpointUrl, string appId)
        {
            _customTextKey = customTextKey;
            _endpointUrl = endpointUrl;
            _appId = appId;
            _httpHandler = httpHandler;
            TestConnectionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task TestConnectionAsync()
        {
            var testQuery = "test";
            await SendPredictionRequestAsync(testQuery);
        }

        public async Task<CustomTextPredictionResponse> GetPredictionAsync(string query)
        {
            if (query.Length > Constants.CustomTextPredictionMaxCharLimit)
            {
                throw new CustomTextPredictionMaxCharExceededException(query.Length);
            }
            // send prediction request
            var operationId = await SendPredictionRequestAsync(query);
            // wait until operation is finished
            CustomTextQueryResponse operationStatus;
            var iterationCounter = 0;
            do
            {
                await Task.Delay(Constants.CustomTextPredictionStatusDelayInMillis);
                operationStatus = await PingStatusAsync(operationId);
            }
            while (iterationCounter < Constants.CustomTextPredictionStatusMaxIterations &&
            (operationStatus.Status == CustomTextPredictionResponseStatus.notstarted ||
            operationStatus.Status == CustomTextPredictionResponseStatus.running));
            // get result
            if (operationStatus.Status == CustomTextPredictionResponseStatus.succeeded)
            {
                var prediction = await GetResultAsync(operationId);
                return prediction;
            }
            else
            {
                if (string.IsNullOrEmpty(operationStatus.ErrorDetails))
                {
                    throw new PredictionOperationFailedException(operationId);
                }
                throw new PredictionOperationFailedException(operationId, operationStatus.ErrorDetails);
            }
        }

        private async Task<string> SendPredictionRequestAsync(string queryText)
        {
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/predictText?log=true&%24expand=classifier%2Cextractor", _endpointUrl, _appId);
            var requestBody = new Dictionary<string, string>
                {
                    { "query", queryText }
                };
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendJsonPostRequestAsync(requestUrl, requestBody, headers, null);
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseContent = JsonConvert.DeserializeObject<CustomTextQueryResponse>(responseString);
                return responseContent.OperationId;
            }
            else
            {
                await HandleExceptionResponseCodesAsync(response, requestUrl);
                return null;
            }
        }

        private async Task<CustomTextQueryResponse> PingStatusAsync(string operationId)
        {
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/operations/{2}/predictText/status", _endpointUrl, _appId, operationId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            var response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseContent = JsonConvert.DeserializeObject<CustomTextQueryResponse>(responseString);
                return responseContent;
            }
            else
            {
                await HandleExceptionResponseCodesAsync(response, requestUrl);
                return null;
            }
        }

        private async Task<CustomTextPredictionResponse> GetResultAsync(string operationId)
        {
            var requestUrl = string.Format("{0}/luis/prediction/v4.0-preview/documents/apps/{1}/slots/production/operations/{2}/predictText", _endpointUrl, _appId, operationId);
            var headers = new Dictionary<string, string>
            {
                ["Ocp-Apim-Subscription-Key"] = _customTextKey
            };
            HttpResponseMessage response = await _httpHandler.SendGetRequestAsync(requestUrl, headers, null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomTextPredictionResponse>(responseString);
            }
            else
            {
                await HandleExceptionResponseCodesAsync(response, requestUrl);
                return null;
            }
        }

        private async Task HandleExceptionResponseCodesAsync(HttpResponseMessage response, string url)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<CustomTextErrorResponse>(responseBody);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException(url, _customTextKey);
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundExcption(errorResponse.Error.Message);
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(errorResponse.Error.Message);
                default:
                    throw new CliException(errorResponse.Error.Message);
            }
        }
    }
}
