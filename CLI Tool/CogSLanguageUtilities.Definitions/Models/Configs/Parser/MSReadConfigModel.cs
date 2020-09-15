using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser
{
    public class MSReadConfigModel
    {
        [JsonProperty(ConfigKeys.MSReadAzureResourceEndpoint)]
        public string CognitiveServiceEndPoint { get; set; }

        [JsonProperty(ConfigKeys.MSReadAzureResourceKey)]
        public string CongnitiveServiceKey { get; set; }
    }
}
