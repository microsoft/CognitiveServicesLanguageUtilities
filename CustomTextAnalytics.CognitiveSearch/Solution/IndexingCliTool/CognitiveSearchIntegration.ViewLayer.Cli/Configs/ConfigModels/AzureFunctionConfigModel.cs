using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs.ConfigModels
{
    public partial class AzureFunctionConfigModel
    {
        [JsonProperty("functionUrl")]
        public string FunctionUrl { get; set; }
    }
}
