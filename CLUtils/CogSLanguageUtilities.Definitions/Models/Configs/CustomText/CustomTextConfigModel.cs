using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText
{
    public class CustomTextConfigModel
    {
        [JsonProperty("authoring")]
        public CustomTextAppConfigModel Authoring { get; set; }

        [JsonProperty("prediction")]
        public CustomTextAppConfigModel Prediction { get; set; }

        public CustomTextConfigModel()
        {
            Authoring = new CustomTextAppConfigModel();
            Prediction = new CustomTextAppConfigModel();
        }
    }
}
