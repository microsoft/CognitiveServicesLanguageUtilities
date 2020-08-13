

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CliTool.Services.Configuration
{
    public class StorageConfigModel
    {
        [JsonProperty("connectionType")]
        public StorageType ConnectionType { get; set; }

        [JsonProperty("localDirectory")]
        public string LocalDirectory { get; set; }

        [JsonProperty("blobContainerName")]
        public string BlobContainerName { get; set; }

        [JsonProperty("blobStorageConnectionString")]
        public string BlobStorageConnectionString { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StorageType
    {
        Local,
        Blob
    }
}
