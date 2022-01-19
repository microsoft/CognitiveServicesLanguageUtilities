using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels
{
    public class Metadata
    {
        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("storageInputContainerName")]
        public string StorageInputContainerName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("multiLingual")]
        public bool MultiLingual { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }
}
