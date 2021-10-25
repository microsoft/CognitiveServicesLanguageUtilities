using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs.ConfigModels
{
    public class CustomTextConfigModel
    {
        [JsonProperty("endpointUrl")]
        public string EndpointUrl { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("deploymentName")]
        public string DeploymentName { get; set; }
    }
}
