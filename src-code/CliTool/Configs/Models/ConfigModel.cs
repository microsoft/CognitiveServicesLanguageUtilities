using Newtonsoft.Json;

namespace CliTool.Services.Configuration
{
    class ConfigModel
    {
        // Blob Storage Connection Configs
        [JsonProperty("blobStorageConnectionString")]
        public string BlobStorageConnectionString { get; set; }
        // MS-Read Connection Configs
        [JsonProperty("cognitiveServiceEndPoint")]
        public string CognitiveServiceEndPoint { get; set; }
        [JsonProperty("congnitiveServiceKey")]
        public string CongnitiveServiceKey { get; set; }
        // Storage Configs
        [JsonProperty("localSourceFolder")]
        public string LocalSourceFolder { get; set; }
        [JsonProperty("localDestinationFolder")]
        public string LocalDestinationFolder { get; set; }
        [JsonProperty("blobSourceContainer")]
        public string BlobSourceContainer { get; set; }
        [JsonProperty("blobDestinationContainer")]
        public string BlobDestinationContainer { get; set; }
        // Storage type
        [JsonProperty("sourceStorageConnectionType")]
        public StorageType SourceStorageConnectionType { get; set; }
        [JsonProperty("destinationStorageConnectionType")]
        public StorageType DestinationStorageConnectionType { get; set; }
    }
}
