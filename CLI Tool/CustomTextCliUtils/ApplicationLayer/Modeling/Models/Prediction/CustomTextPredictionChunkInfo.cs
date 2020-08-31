using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse;
using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction
{
    class CustomTextPredictionChunkInfo
    {
        [JsonProperty(Order = 0)]
        public int ChunkNumber { get; set; }
        [JsonProperty(Order = 1)]
        public int CharCount { get; set; }
        [JsonProperty(Order = 2)]
        public int StartPage { get; set; }
        [JsonProperty(Order = 3)]
        public int EndPage { get; set; }
        [JsonProperty(Order = 4)]
        public string InnerText { get; set; }
        [JsonProperty(Order = 5)]
        public CustomTextPredictionResponse CustomTextPredictionResponse { get; set; }
    }
}
