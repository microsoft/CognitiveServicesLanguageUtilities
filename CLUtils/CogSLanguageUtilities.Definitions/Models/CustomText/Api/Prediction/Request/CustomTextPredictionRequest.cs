using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Request
{
    public class CustomTextPredictionRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        public CustomTextPredictionRequest(string query)
        {
            Query = query;
        }
    }
}
