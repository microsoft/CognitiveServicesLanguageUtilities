using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer
{
    public class IndexerConfiguration
    {
        [JsonProperty("parsingMode", NullValueHandling = NullValueHandling.Ignore)]
        public string ParsingMode { get; set; }

        [JsonProperty("indexedFileNameExtensions", NullValueHandling = NullValueHandling.Ignore)]
        public string IndexedFileNameExtensions { get; set; }

        [JsonProperty("imageAction", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageAction { get; set; }

        [JsonProperty("dataToExtract", NullValueHandling = NullValueHandling.Ignore)]
        public string DataToExtract { get; set; }

        [JsonProperty("executionEnvironment", NullValueHandling = NullValueHandling.Ignore)]
        public string ExecutionEnvironment { get; set; }
    }
}
