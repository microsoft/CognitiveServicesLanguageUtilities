using Newtonsoft.Json;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels
{
    public class AnalyzeTextProject
    {
        [JsonProperty("api-version")]
        public string ApiVersion { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("assets")]
        public Assets Assets { get; set; }

        public Extractor[] getResultExtractors()
        {
            return Assets.Extractors;
        }
    }
}
