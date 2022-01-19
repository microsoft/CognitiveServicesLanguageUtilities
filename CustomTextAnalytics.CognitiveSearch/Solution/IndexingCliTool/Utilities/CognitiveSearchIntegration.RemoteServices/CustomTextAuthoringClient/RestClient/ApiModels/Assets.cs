using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels
{
    public class Assets
    {
        [JsonProperty("extractors")]
        public Extractor[] Extractors { get; set; }
    }

    public class Extractor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
