using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics
{
    public class TextAnalyticsConfigModel
    {
        [JsonProperty(ConfigKeys.TextAnalyticsAzureResourceEndpoint)]
        public string AzureResourceEndpoint { get; set; }

        [JsonProperty(ConfigKeys.TextAnalyticsAzureResourceKey)]
        public string AzureResourceKey { get; set; }

        [JsonProperty(ConfigKeys.TextAnalyticsDefaultLanguage)]
        public string DefaultLanguage { get; set; }

        [JsonProperty("default_operations")]
        public TextAnalyticsDefaultOperationsConfigModel DefaultOperations { get; set; }

        public TextAnalyticsConfigModel()
        {
            DefaultOperations = new TextAnalyticsDefaultOperationsConfigModel();
        }
    }
}
