using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Error
{
    public class CustomTextErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
