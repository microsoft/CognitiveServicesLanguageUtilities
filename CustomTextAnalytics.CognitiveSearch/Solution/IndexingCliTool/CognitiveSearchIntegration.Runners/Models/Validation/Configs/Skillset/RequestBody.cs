using Newtonsoft.Json;

namespace CognitiveSearchIntegration.Runners.Models.Validation.Configs.Skillset
{
    public class RequestBody
    {
        [JsonProperty("Values")]
        public RequestValue[] Values { get; set; }
    }

    public class RequestValue
    {
        [JsonProperty("RecordId")]
        public string RecordId { get; set; }

        [JsonProperty("Data")]
        public RequestData Data { get; set; }
    }

    public class RequestData
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
