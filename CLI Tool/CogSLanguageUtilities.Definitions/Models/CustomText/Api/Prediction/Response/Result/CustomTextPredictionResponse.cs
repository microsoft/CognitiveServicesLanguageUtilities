using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Result
{
    public class CustomTextPredictionResponse
    {
        [JsonProperty(PropertyName = "Prediction", Order = 0)]
        public PredictionContent Prediction { get; set; }
    }
}