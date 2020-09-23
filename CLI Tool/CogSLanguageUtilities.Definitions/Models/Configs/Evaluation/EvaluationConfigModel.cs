using Newtonsoft.Json;
namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Evaluation
{
    public class EvaluationConfigModel
    {
        [JsonProperty("labeledExamplesApp")]
        public LabeledExamplesAppConfigModel LabeledExamplesApp { get; set; }

        public EvaluationConfigModel()
        {
            LabeledExamplesApp = new LabeledExamplesAppConfigModel();
        }
    }
}
