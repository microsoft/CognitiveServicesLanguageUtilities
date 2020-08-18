

using Newtonsoft.Json;

namespace CliTool.Services.Configuration.Models
{
    public class MSReadConfigModel
    {
        [JsonProperty("cognitiveServiceEndPoint")]
        public string CognitiveServiceEndPoint { get; set; }

        [JsonProperty("congnitiveServiceKey")]
        public string CongnitiveServiceKey { get; set; }
    }
}
