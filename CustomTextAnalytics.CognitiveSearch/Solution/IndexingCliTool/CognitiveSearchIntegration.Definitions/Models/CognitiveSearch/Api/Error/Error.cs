using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Error
{
    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
