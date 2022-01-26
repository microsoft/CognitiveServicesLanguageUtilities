using Newtonsoft.Json;

namespace CognitiveSearchIntegration.Runners.Models.ConfigsModel
{
    public class ConfigModel
    {
        [JsonProperty("blobStorage")]
        public BlobStorageConfigModel BlobStorage { get; set; }

        [JsonProperty("cognitiveSearch")]
        public CognitiveSearchConfigModel CognitiveSearch { get; set; }

        [JsonProperty("azureFunction")]
        public AzureFunctionConfigModel AzureFunction { get; set; }

        [JsonProperty("customText")]
        public CustomTextConfigModel CustomText { get; set; }
    }
}
