using Microsoft.LuisModelEvaluation.Models.Result;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation
{
    public class BatchTestingDocumentStats
    {
        public BatchTestingDocumentStats()
        {
            FalsePositiveEntities = new List<BatchTestingEntityDetails>();
            FalseNegativeEntities = new List<BatchTestingEntityDetails>();
        }

        [JsonProperty(PropertyName = "text")]
        public string DocumentText { get; set; }

        [JsonProperty(PropertyName = "predictedClassNames")]
        public List<string> PredictedClasstNames { get; set; }

        [JsonProperty(PropertyName = "labeledClasstNames")]
        public List<string> LabeledClassNames { get; set; }

        [JsonProperty(PropertyName = "falsePositiveEntities")]
        public List<BatchTestingEntityDetails> FalsePositiveEntities { get; set; }

        [JsonProperty(PropertyName = "falseNegativeEntities")]
        public List<BatchTestingEntityDetails> FalseNegativeEntities { get; set; }
    }
}
