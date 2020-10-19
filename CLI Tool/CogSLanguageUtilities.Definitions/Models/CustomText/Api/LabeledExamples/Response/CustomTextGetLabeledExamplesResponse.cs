using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response
{
    public class CustomTextGetLabeledExamplesResponse
    {
        [JsonProperty("examples")]
        public List<Example> Examples { get; set; }

        [JsonProperty("nextPageLink")]
        public string NextPageLink { get; set; }
    }
}
