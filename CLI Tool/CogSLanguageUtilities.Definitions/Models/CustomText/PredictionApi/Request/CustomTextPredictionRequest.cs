using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Request
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
