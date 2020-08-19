using Newtonsoft.Json;

namespace CustomTextCliUtils.Configs.Models.Storage
{
    public class LocalStorageConfigModel
    {
        [JsonProperty("sourceDirectory")]
        public string SourceDirectory { get; set; }

        [JsonProperty("destinationDirectory")]
        public string DestinationDirectory { get; set; }
    }
}