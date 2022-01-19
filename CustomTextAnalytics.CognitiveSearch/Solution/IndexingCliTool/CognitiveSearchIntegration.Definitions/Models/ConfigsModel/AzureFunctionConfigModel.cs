using Newtonsoft.Json;

namespace CognitiveSearchIntegration.Runners.Models.ConfigsModel
{
    public partial class AzureFunctionConfigModel
    {
        [JsonProperty("functionUrl")]
        public string FunctionUrl { get; set; }
    }
}
