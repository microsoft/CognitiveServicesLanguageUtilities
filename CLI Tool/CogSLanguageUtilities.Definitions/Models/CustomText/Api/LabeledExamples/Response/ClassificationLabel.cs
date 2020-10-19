using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response
{
    public class ClassificationLabel
    {
        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("label")]
        public bool Label { get; set; }
    }
}
