using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse
{
    public class CustomTextPredictionResponse
    {
        [JsonProperty(PropertyName = "Prediction", Order = 0)]
        public PredictionContent Prediction { get; set; }
    }
}