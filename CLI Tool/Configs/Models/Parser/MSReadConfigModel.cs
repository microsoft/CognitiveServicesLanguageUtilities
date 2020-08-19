

using Newtonsoft.Json;

namespace CustomTextCliUtils.Configs.Models.Parser
{
    public class MSReadConfigModel
    {
        [JsonProperty("cognitiveServiceEndPoint")]
        public string CognitiveServiceEndPoint { get; set; }

        [JsonProperty("congnitiveServiceKey")]
        public string CongnitiveServiceKey { get; set; }
    }
}
