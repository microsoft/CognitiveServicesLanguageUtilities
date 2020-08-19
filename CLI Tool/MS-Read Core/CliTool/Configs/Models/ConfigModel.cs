
using CustomTextCliUtils.Configs.Models.Parser;
using CustomTextCliUtils.Configs.Models.Storage;
using Newtonsoft.Json;

namespace CustomTextCliUtils.Configs.Models
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public StorageConfigModel Storage { get; set; }

        [JsonProperty("parser")]
        public ParserConfigModel Parser { get; set; }
    }

    public class ParserConfigModel
    {
        [JsonProperty("MSRead")]
        public MSReadConfigModel MsRead { get; set; }
    }

    public class StorageConfigModel
    {
        [JsonProperty("blob")]
        public BlobStorageConfigModel Blob { get; set; }

        [JsonProperty("local")]
        public LocalStorageConfigModel Local { get; set; }
    }
}
