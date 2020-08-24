using Newtonsoft.Json;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public StorageConfigModel Storage { get; set; }

        [JsonProperty("parser")]
        public ParserConfigModel Parser { get; set; }

        [JsonProperty("chunker")]
        public ChunkerConfigModel Chunker { get; set; }

        [JsonProperty("prediction")]
        public PredictionConfigModel Prediction { get; set; }

        public ConfigModel() {
            Storage = new StorageConfigModel();
            Parser = new ParserConfigModel();
            Chunker = new ChunkerConfigModel();
            Prediction = new PredictionConfigModel();
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
