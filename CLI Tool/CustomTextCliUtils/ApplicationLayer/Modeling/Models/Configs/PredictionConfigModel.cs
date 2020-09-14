using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class PredictionConfigModel
    {
        [JsonProperty(ConfigKeys.CustomTextAzureResourceKey)]
        public string CustomTextKey { get; set; }

        [JsonProperty(ConfigKeys.CustomTextAzureResourceEndpoint)]
        public string EndpointUrl { get; set; }

        [JsonProperty(ConfigKeys.CustomTextAppId)]
        public string AppId { get; set; }
    }
}
