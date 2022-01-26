using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Indexer
{
    public class IndexerParameters
    {
        [JsonProperty("maxFailedItems", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxFailedItems { get; set; }

        [JsonProperty("batchSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? BatchSize { get; set; }

        [JsonProperty("configuration", NullValueHandling = NullValueHandling.Ignore)]
        public IndexerConfiguration Configuration { get; set; }
    }
}
