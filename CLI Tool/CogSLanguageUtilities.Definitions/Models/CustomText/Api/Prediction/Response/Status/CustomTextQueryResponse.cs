using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.CustomText;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Status
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

        [JsonProperty(PropertyName = "errorDetails", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDetails { get; set; }
    }
}