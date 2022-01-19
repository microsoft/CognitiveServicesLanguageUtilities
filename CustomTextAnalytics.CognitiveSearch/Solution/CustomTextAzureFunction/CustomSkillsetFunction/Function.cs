using CustomSkillsetFunction.Helpers;
using CustomSkillsetFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.CognitiveSearch.CustomSkillset.CustomText.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomSkillsetFunction
{
    public static class Function
    {
        [FunctionName("customtext-extractor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ILogger log,
            ExecutionContext executionContext)
        {
            // some meta data
            log.LogInformation("LUIS-D Extractor and Classifier function");
            var functionName = executionContext.FunctionName;

            // input mapping
            var requestRecords = ViewModelMapper.GetRequestRecords(request);
            if (requestRecords == null)
            {
                return new BadRequestObjectResult($"Invalid request body!");
            }

            // extract credentials
            var credentials = CredentialsHelper.GetProjectCredentials(request.Headers);

            if (string.IsNullOrEmpty(credentials.ResourceCredentials.EndpointUrl) ||
                string.IsNullOrEmpty(credentials.ResourceCredentials.Key))
            {
                return new BadRequestObjectResult("please provide a valid Custom Text resource endpoint and key!");
            }

            try
            {
                // create input documents
                var documents = new Dictionary<string, string>();
                requestRecords.ToList().ForEach(record => documents.Add(record.RecordId, record.Data["text"] as string));

                // analyze custom text
                var customTextPredictionService = new CustomTextPredictionService(credentials.ResourceCredentials);
                var targetTasks = CustomTextTaskHelper.InitializeTargetTasks(credentials.Projects);
                if (targetTasks.Count == 0)
                {
                    return new BadRequestObjectResult("please provide one or more Custom Text projects to use for document analysis!");
                }
                var responseRecords = await customTextPredictionService.AnalyzeDocuments(documents, targetTasks);

                // create custom skillset response
                var customSkillsetResponse = ViewModelMapper.CreateResponseData(responseRecords);
                return new OkObjectResult(customSkillsetResponse);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}