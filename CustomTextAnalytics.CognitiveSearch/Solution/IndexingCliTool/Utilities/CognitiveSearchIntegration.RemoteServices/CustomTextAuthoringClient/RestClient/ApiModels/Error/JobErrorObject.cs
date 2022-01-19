using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels.Error
{
    public class JobErrorObject
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
