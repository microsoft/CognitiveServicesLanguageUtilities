using Newtonsoft.Json;
using System.Collections.Generic;

namespace CognitiveSearchIntegration.Runners.Models.Validation.Configs.Skillset
{
    public class Response
    {
        [JsonProperty("values")]
        public ResponseValue[] Values { get; set; }
    }

    public class ResponseValue
    {
        [JsonProperty("recordId")]
        public string RecordId { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("warnings")]
        public object[] Warnings { get; set; }
    }
}
