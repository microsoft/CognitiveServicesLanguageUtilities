using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Error
{
    public class CognitiveSearchErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
