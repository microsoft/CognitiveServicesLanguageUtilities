using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient
{
    public class CustomTextAuthoringClient
    {
        private CustomTextAuthoringRestClient _restClient;

        public CustomTextAuthoringClient(string endpointUrl, string serviceKey)
        {
            _restClient = new CustomTextAuthoringRestClient(endpointUrl, serviceKey);
        }
        public async Task<Extractor[]> ExportProjectEntities(string projectName)
        {
            string jobId = await _restClient.StartProjectExportAsync(projectName);
            await WaitForJobUntilDone(projectName, jobId);
            var jobStatus = await _restClient.GetProjectExportJobStatus(projectName, jobId);
            var jobResult = await _restClient.GetProjectExportJobResult(jobStatus.ResultUrl.AbsoluteUri);
            return jobResult.Assets.Extractors;
        }
        private async Task<bool> WaitForJobUntilDone(string projectName, string jobId, int timeoutInSeconds = 0, int pollingIntervalInSeconds = 5)
        {
            // prepare algorithm
            var doneValuesList = Enum.GetNames(typeof(JobDoneStatus)).Select(value => value.ToLower());
            var jobDoneStatusSet = new HashSet<string>(doneValuesList);

            var startTimeStamp = DateTime.Now;
            while (true)
            {
                // check job status is done
                var jobInfo = await _restClient.GetProjectExportJobStatus(projectName, jobId);
                var jobStatus = jobInfo.Status.ToLower();
                if (jobDoneStatusSet.Contains(jobStatus))
                {
                    return true;
                }

                // check for timeouts
                if (timeoutInSeconds > 0)
                {
                    var nowTimeStamp = DateTime.Now;
                    var diff = nowTimeStamp.Subtract(startTimeStamp).TotalSeconds;
                    if (diff > timeoutInSeconds)
                    {
                        break;
                    }
                }

                // sleep for a while
                Thread.Sleep(pollingIntervalInSeconds);
            }
            return false;
        }
    }
}