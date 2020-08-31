using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
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

        public ConfigModel()
        {
            Storage = new StorageConfigModel();
            Parser = new ParserConfigModel();
            Chunker = new ChunkerConfigModel();
            Prediction = new PredictionConfigModel();
        }
    }
}
