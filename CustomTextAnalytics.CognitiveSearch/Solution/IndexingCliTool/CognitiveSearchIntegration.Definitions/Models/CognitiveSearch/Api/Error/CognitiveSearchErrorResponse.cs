using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Error
{
    public class CognitiveSearchErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
