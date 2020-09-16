using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.LuisModelEvaluation.Models.Result
{
    public class UtteranceStats
    {
        public UtteranceStats()
        {
            FalsePositiveEntities = new List<EntityNameAndLocation>();
            FalseNegativeEntities = new List<EntityNameAndLocation>();
        }

        [JsonProperty(PropertyName = "text")]
        public string UtteranceText { get; set; }

        [JsonProperty(PropertyName = "predictedIntentName")]
        public List<string> PredictedIntentNames { get; set; }

        [JsonProperty(PropertyName = "labeledIntentName")]
        public List<string> LabeledIntentNames { get; set; }

        [JsonProperty(PropertyName = "falsePositiveEntities")]
        public List<EntityNameAndLocation> FalsePositiveEntities { get; set; }

        [JsonProperty(PropertyName = "falseNegativeEntities")]
        public List<EntityNameAndLocation> FalseNegativeEntities { get; set; }
    }
}