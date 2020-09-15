using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics
{
    public class TextAnalyticsDefaultOperationsConfigModel
    {
        [JsonProperty(ConfigKeys.TextAnalyticsSentiment)]
        public bool Sentiment { get; set; }

        [JsonProperty(ConfigKeys.TextAnalyticsNer)]
        public bool Ner { get; set; }

        [JsonProperty(ConfigKeys.TextAnalyticsKeyphrase)]
        public bool Keyphrase { get; set; }
    }
}
