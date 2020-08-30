using Newtonsoft.Json;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction
{
    public class CustomTextErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
