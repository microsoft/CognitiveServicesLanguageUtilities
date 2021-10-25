using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs.ConfigModels
{
    public partial class BlobStorageConfigModel
    {
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty("containerName")]
        public string ContainerName { get; set; }
    }
}
