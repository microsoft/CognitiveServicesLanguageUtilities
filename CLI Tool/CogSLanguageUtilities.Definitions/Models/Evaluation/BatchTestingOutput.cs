using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation
{
    public class BatchTestingOutput
    {
        /// <summary>
        /// Gets or sets a list of statistics about each classification model
        /// </summary>
        [JsonProperty(PropertyName = "classificationModelsStats")]
        public IReadOnlyList<BatchTestingModelStats> ClassificationModelsStats { get; set; }

        /// <summary>
        /// Gets or sets a list of statistics about each entity model
        /// </summary>
        [JsonProperty(PropertyName = "entityModelsStats")]
        public IReadOnlyList<BatchTestingModelStats> EntityModelsStats { get; set; }

        /// <summary>
        /// Gets or sets a list of statistics about each document
        /// </summary>
        [JsonProperty(PropertyName = "documentsStats")]
        public IReadOnlyList<BatchTestingDocumentStats> DocumentsStats { get; set; }
    }
}
