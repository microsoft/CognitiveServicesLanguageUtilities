using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response
{
    public class CustomTextGetModelsResponse
    {
        [JsonProperty("models")]
        public List<CustomTextModel> Models { get; set; }
    }
}
