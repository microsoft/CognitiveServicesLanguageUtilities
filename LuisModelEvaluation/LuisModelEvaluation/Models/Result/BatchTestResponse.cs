using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.LuisModelEvaluation.Models.Result
{
    public class BatchTestResponse
    {
        /// <summary>
        /// Gets or sets a list of statistics about each intent model.
        /// </summary>
        [JsonProperty(PropertyName = "intentModelsStats")]
        public IReadOnlyList<ModelStats> IntentModelsStats { get; set; }

        /// <summary>
        /// Gets or sets a list of statistics about each entity model.
        /// </summary>
        [JsonProperty(PropertyName = "entityModelsStats")]
        public IReadOnlyList<ModelStats> EntityModelsStats { get; set; }

        /// <summary>
        /// Gets or sets a list of statistics about each Utterance.
        /// </summary>
        [JsonProperty(PropertyName = "utterancesStats")]
        public IReadOnlyList<UtteranceStats> UtterancesStats { get; set; }
    }
}