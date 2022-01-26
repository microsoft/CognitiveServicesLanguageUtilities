using Newtonsoft.Json;

namespace CognitiveSearchIntegration.Runners.Models.ConfigsModel
{
    public partial class CognitiveSearchConfigModel
    {
        [JsonProperty("endpointUrl")]
        public string EndpointUrl { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}
