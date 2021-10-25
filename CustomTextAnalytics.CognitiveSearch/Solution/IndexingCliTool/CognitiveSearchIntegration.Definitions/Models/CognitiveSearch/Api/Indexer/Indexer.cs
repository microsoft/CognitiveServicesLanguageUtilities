using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer
{
    public class Indexer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dataSourceName")]
        public string DataSourceName { get; set; }

        [JsonProperty("targetIndexName")]
        public string TargetIndexName { get; set; }

        [JsonProperty("skillsetName")]
        public string SkillsetName { get; set; }

        [JsonProperty("fieldMappings")]
        public List<IndexerFieldMapping> FieldMappings { get; set; }

        [JsonProperty("outputFieldMappings")]
        public List<IndexerFieldMapping> OutputFieldMappings { get; set; }

        [JsonProperty("parameters")]
        public IndexerParameters Parameters;
    }
}
