using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Evaluation
{
    public class LabeledExamplesAppConfigModel
    {
        [JsonProperty(ConfigKeys.EvaluationLabeledExamplesAppAzureResourceKey)]
        public string AzureResourceKey { get; set; }

        [JsonProperty(ConfigKeys.EvaluationLabeledExamplesAppAzureResourceEndpoint)]
        public string AzureResourceEndpoint { get; set; }

        [JsonProperty(ConfigKeys.EvaluationLabeledExamplesAppId)]
        public string AppId { get; set; }
    }
}
