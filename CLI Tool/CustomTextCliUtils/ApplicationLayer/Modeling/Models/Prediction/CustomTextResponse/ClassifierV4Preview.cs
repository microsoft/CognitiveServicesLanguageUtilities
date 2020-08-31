using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse
{
    public class ClassifierV4Preview
    {
        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Ignore, Order = 0)]
        public float? Score { get; set; }
    }
}
