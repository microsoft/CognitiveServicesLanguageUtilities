using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Indexer
{
    public class IndexerFieldMapping
    {
        [JsonProperty("sourceFieldName")]
        public string SourceFieldName { get; set; }

        [JsonProperty("targetFieldName")]
        public string TargetFieldName { get; set; }

        [JsonProperty("mappingFunction", NullValueHandling = NullValueHandling.Ignore)]
        public MappingFunction MappingFunction { get; set; }
    }
}
