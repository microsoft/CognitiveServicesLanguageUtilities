using Newtonsoft.Json;

namespace CliTool.Configs
{
    public class BlobStorageConfigModel
    {
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty("sourceContainer")]
        public string SourceContainer { get; set; }

        [JsonProperty("destinationContainer")]
        public string DestinationContainer { get; set; }
    }
}