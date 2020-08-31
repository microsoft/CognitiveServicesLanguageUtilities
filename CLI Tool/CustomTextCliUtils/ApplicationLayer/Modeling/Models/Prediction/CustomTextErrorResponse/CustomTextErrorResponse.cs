using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextErrorResponse
{
    public class CustomTextErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
