using Newtonsoft.Json;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction
{
    public class CustomTextQueryRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        public CustomTextQueryRequest(string query)
        {
            Query = query;
        }
    }
}
