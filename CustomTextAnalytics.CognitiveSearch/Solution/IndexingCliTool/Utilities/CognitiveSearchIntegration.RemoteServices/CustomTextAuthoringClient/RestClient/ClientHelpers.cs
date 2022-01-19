using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels.Error;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient
{
    class ClientHelpers
    {
        public async Task HandleApiErrorResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                var errorObject = JsonHandler.DeserializeObject<JobErrorObject>(await response.Content.ReadAsStringAsync());
                //throw new CliException(errorObject.Error.Message);
            }
        }


        public string ExtractJobIdFromLocationHeader(string operationLocationHeader)
        {
            string last = operationLocationHeader.Split("/").ToList().Last();
            return last.Split("?").ToList().First();
        }
    }
}
