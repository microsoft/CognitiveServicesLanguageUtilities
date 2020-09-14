using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
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
