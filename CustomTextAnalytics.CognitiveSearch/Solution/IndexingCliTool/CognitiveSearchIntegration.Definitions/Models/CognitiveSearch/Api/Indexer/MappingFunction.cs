using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer
{
    public class MappingFunction
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
