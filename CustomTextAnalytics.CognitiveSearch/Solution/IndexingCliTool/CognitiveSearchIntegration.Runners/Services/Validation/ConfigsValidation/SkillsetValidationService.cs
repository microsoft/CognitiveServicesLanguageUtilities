using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using CognitiveSearchIntegration.Runners.Models.Validation.Configs.Skillset;
using Microsoft.CognitiveSearchIntegration.Definitions.Configs;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation
{
    public class SkillsetValidationService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task ValidateCustomSkillsetConfigsAsync(
            string azureFunctionUrl,
            CustomTextConfigModel skillsetConfigs,
            SelectedProjects selectedProjects)
        {
            try
            {
                await ValidateCustomSkillsetConfigsAsyncInternal(azureFunctionUrl, skillsetConfigs, selectedProjects);
            }
            catch (Exception)
            {
                throw new Exception("Custom Text App Credentials Are Invalid!");
            }
        }

        public async Task ValidateCustomSkillsetConfigsAsyncInternal(
            string azureFunctionUrl,
            CustomTextConfigModel skillsetConfigs,
            SelectedProjects selectedProjects)
        {
            // send request
            var body = new RequestBody()
            {
                Values = new RequestValue[]
                {
                new RequestValue(){
                    RecordId = "123",
                    Data = new RequestData(){
                        Text = "some testing text"
                    }
                }
                }
            };
            var headers = CreateHeaders(skillsetConfigs, selectedProjects);
            var response = await SendJsonPostRequestAsync(url: azureFunctionUrl, body: body, headers: headers, parameters: null);

            // validate response
            var result = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
            ValidateResponse(result);
        }

        private static Dictionary<string, string> CreateHeaders(CustomTextConfigModel skillsetConfigs, SelectedProjects selectedProjects)
        {
            var headers = new Dictionary<string, string>()
            {
                [Constants.CustomTextResourceEndpointHeader] = skillsetConfigs.Resource.Endpoint,
                [Constants.CustomTextResourceKeyHeader] = skillsetConfigs.Resource.Key,
            };

            if (selectedProjects.IsSelected_EntityRecognitionProject)
            {
                headers.Add(Constants.EntityRecognitionProjectNameHeader, skillsetConfigs.Projects.EntityRecognition.ProjectName);
                headers.Add(Constants.EntityRecognitionDeploymentNameHeader, skillsetConfigs.Projects.EntityRecognition.DeploymentName);
            }
            if (selectedProjects.IsSelected_SingleClassificationProject)
            {
                headers.Add(Constants.SingleClassificationProjectNameHeader, skillsetConfigs.Projects.SingleClassification.ProjectName);
                headers.Add(Constants.SingleClassificationDeploymentNameHeader, skillsetConfigs.Projects.SingleClassification.DeploymentName);
            }
            if (selectedProjects.IsSelected_MultiClassificationProject)
            {
                headers.Add(Constants.MultiClassificationProjectNameHeader, skillsetConfigs.Projects.MultiClassification.ProjectName);
                headers.Add(Constants.MultiClassificationDeploymentNameHeader, skillsetConfigs.Projects.MultiClassification.DeploymentName);
            }
            return headers;
        }

        private void ValidateResponse(Response response)
        {
            // simply try to access properties
            if (response.Values[0].Errors.Length > 0 || // this indicates custom text credentials are invalid
                response.Values[0].Data == null ||
                response.Values[0].Data.Values == null ||
                response.Values[0].Errors == null ||
                response.Values[0].Warnings == null)
            {
                throw new Exception();
            }
        }
        private async Task<HttpResponseMessage> SendJsonPostRequestAsync(string url, object body, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            var urlWithParameters = parameters == null ? url : CreateUrlWithParameters(url, parameters);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, urlWithParameters))
            {
                PopulateRequestMessageHeaders(headers, requestMessage);
                var requestBodyAsJson = JsonConvert.SerializeObject(body);
                requestMessage.Content = new StringContent(requestBodyAsJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
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
