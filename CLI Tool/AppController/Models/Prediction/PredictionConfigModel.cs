using CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace CustomTextCliUtils.AppController.Models.Prediction
{
    public class PredictionConfigModel
    {
        [JsonProperty(ConfigKeys.PredictionCustomTextKey)]
        public string CustomTextKey { get; set; }

        [JsonProperty(ConfigKeys.PredictionEndpointUrl)]
        public string EndpointUrl { get; set; }

        [JsonProperty(ConfigKeys.PredictionAppId)]
        public string AppId { get; set; }

        [JsonProperty(ConfigKeys.PredictionVersionId)]
        public string VersionId { get; set; }
    }
}
