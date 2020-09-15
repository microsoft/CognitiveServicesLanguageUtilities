using Newtonsoft.Json;
using System;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Evaluation
{
    public class LabeledExamplesAppConfigModel
    {
        [JsonProperty("azure-resource-key")]
        public string AzureResourceKey { get; set; }

        [JsonProperty("azure-resource-endpoint")]
        public string AzureResourceEndpoint { get; set; }

        [JsonProperty("app-id")]
        public string AppId { get; set; }
    }
}
