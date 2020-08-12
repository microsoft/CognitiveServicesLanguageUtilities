using Newtonsoft.Json;

namespace CliTool.Services.Configuration
{
    class ConfigModel
    {
        // Blob Storage Connection Configs
        [JsonProperty("BlobStorageConnectionString")]
        public string BlobStorageConnectionString { get; set; }
        // MS-Read Connection Configs
        [JsonProperty("CognitiveServiceEndPoint")]
        public string CognitiveServiceEndPoint { get; set; }
        [JsonProperty("CongnitiveServiceKey")]
        public string CongnitiveServiceKey { get; set; }
        // Storage Configs
        [JsonProperty("LocalSourceFolder")]
        public string LocalSourceFolder { get; set; }
        [JsonProperty("LocalDestinationFolder")]
        public string LocalDestinationFolder { get; set; }
        [JsonProperty("BlobSourceContainer")]
        public string BlobSourceContainer { get; set; }
        [JsonProperty("BlobDestinationContainer")]
        public string BlobDestinationContainer { get; set; }
        // Storage type
        [JsonProperty("SourceStorageConnectionType")]
        public StorageType SourceStorageConnectionType { get; set; }
        [JsonProperty("DestinationStorageConnectionType")]
        public StorageType DestinationStorageConnectionType { get; set; }
    }
}
