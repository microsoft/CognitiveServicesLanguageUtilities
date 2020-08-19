using CustomTextCliUtils.AppController.Models.Parser;
using CustomTextCliUtils.AppController.Models.Storage;
using Newtonsoft.Json;

namespace CustomTextCliUtils.AppController.Models
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public StorageConfigModel Storage { get; set; }

        [JsonProperty("parser")]
        public ParserConfigModel Parser { get; set; }

        public ConfigModel() {
            Storage = new StorageConfigModel();
            Parser = new ParserConfigModel();
        }
    }

    public class ParserConfigModel
    {
        [JsonProperty("MSRead")]
        public MSReadConfigModel MsRead { get; set; }

        public ParserConfigModel() {
            MsRead = new MSReadConfigModel();
        }
    }

    public class StorageConfigModel
    {
        [JsonProperty("blob")]
        public BlobStorageConfigModel Blob { get; set; }

        [JsonProperty("local")]
        public LocalStorageConfigModel Local { get; set; }

        public StorageConfigModel() {
            Blob = new BlobStorageConfigModel();
            Local = new LocalStorageConfigModel();
        }
    }
}
