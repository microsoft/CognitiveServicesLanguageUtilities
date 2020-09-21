using System.Collections.Generic;

namespace Microsoft.LuisModelEvaluation.Models.Result
{
    public class QueryStats
    {
        public QueryStats()
        {
            FalsePositiveEntities = new List<EntityNameAndLocation>();
            FalseNegativeEntities = new List<EntityNameAndLocation>();
        }

        public string QueryText { get; set; }

        public List<string> PredictedClassNames { get; set; }

        public List<string> LabeledClassNames { get; set; }

        public List<EntityNameAndLocation> FalsePositiveEntities { get; set; }

        public List<EntityNameAndLocation> FalseNegativeEntities { get; set; }
    }
}