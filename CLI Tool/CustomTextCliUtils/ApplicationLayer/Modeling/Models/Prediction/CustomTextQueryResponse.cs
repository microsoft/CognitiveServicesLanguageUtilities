using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction;
using Newtonsoft.Json;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction
{

    public class CustomTextQueryResponse
    {
        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("status")]
        public CustomTextPredictionResponseStatus Status { get; set; }

        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonProperty("lastActionDateTime")]
        public string LastActionDateTime { get; set; }
    }
}