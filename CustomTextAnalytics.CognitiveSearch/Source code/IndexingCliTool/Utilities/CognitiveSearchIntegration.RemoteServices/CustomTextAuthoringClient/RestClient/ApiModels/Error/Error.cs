using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels.Error
{
    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
