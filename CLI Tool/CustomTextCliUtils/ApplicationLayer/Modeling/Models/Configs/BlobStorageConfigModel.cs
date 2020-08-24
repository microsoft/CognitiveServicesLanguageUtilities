using CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class BlobStorageConfigModel
    {
        [JsonProperty(ConfigKeys.BlobStorageConnectionstring)]
        public string ConnectionString { get; set; }

        [JsonProperty(ConfigKeys.BlobStorageSourceContainer)]
        public string SourceContainer { get; set; }

        [JsonProperty(ConfigKeys.BlobStorageDestinationContainer)]
        public string DestinationContainer { get; set; }
    }
}