using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction
{
    public class CustomTextPredictionResponse
    {
        [JsonProperty(PropertyName = "Prediction", Order = 0)]
        public PredictionContent Prediction { get; set; }
    }

    public class PredictionContent
    {
        [JsonProperty(PropertyName = "positiveClassifiers", Order = 0)]
        public List<string> PositiveClassifiers { get; set; }

        [JsonProperty(PropertyName = "classifiers", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, ClassifierV4Preview> Classifiers { get; set; }


        [JsonProperty(PropertyName = "extractors", Order = 2)]
        public JObject Extractors { get; set; }
    }

    public class ClassifierV4Preview
    {
        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Ignore, Order = 0)]
        public float? Score { get; set; }
    }
}