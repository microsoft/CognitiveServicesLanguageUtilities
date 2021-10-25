using Newtonsoft.Json;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Models.Configs
{
    public class TextAnalyticsConfigModel
    {
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }
}
