using CustomTextAnalytics.MiniSDK.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.CognitiveSearch.CustomSkillset.CustomText.Helpers;
using Microsoft.CognitiveSearch.CustomSkillset.CustomText.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            log.LogInformation("LUIS-D Extractor and Classifier function");
            var functionName = executionContext.FunctionName;

            // input mapping
            IEnumerable<RequestRecord> requestRecords;
            try
            {
                requestRecords = HttpModelHelper.GetRequestRecords(request);
                if (requestRecords == null || requestRecords.Count() == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return new BadRequestObjectResult($"Invalid request body!");
            }

            // get custom text app secrets from header
            var customtextEndpoint = request.Headers["endpointUrl"];
            var customtextKey = request.Headers["apiKey"];
            var customtextProjectName = request.Headers["projectName"];
            var customtextDeploymentName = request.Headers["deploymentName"];

            if (string.IsNullOrEmpty(customtextEndpoint) || string.IsNullOrEmpty(customtextEndpoint) || string.IsNullOrEmpty(customtextEndpoint) || string.IsNullOrEmpty(customtextEndpoint))
            {
                return new BadRequestObjectResult("One of or more of Custom Text app secrets wasn't provided in the request headers!");
            }

            // init sdk client
            var customTextClient = new CustomTextAnalyticsClient(customtextEndpoint, customtextKey);

            // process input data
            var response = new CustomSkillsetResponse();
            var responseErrors = new StringBuilder();

            foreach (var inRecord in requestRecords)
            {
                var outRecord = new ResponseRecord() { RecordId = inRecord.RecordId };
                try
                {
                    var text = inRecord.Data["text"] as string;
                    var entities = await customTextClient.AnalyzeCustomEntitiesAsync(text, customtextProjectName, customtextDeploymentName);
                    entities.ToList().ForEach(entity =>
                    {
                        if (!outRecord.Data.ContainsKey(entity.Category))
                        {
                            outRecord.Data.Add(entity.Category, entity.Text);
                        }
                    });
                }
                catch (Exception e)
                {
                    outRecord.Errors.Add(new ErrorWarning() { Message = $"Error processing the request record : {e.Message}" });
                }

                response.Values.Add(outRecord);
            }


            if (responseErrors.Length > 0)
            {
                return new BadRequestObjectResult(responseErrors.ToString());
            }
            return new OkObjectResult(response);
        }
    }
}
