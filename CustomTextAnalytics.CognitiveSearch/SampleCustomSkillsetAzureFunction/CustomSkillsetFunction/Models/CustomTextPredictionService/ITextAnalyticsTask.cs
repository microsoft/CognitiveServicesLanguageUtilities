using Azure.AI.TextAnalytics;
using CustomSkillsetFunction.ViewModels.Response;
using System.Collections.Generic;

namespace CustomSkillsetFunction.Models.CustomTextPredictionService
{
    public interface ITextAnalyticsTask
    {
        public void Inject(TextAnalyticsActions textAnalyticsActions);
        public List<ResponseRecord> ExtractResult(AnalyzeActionsResult analyzeActionsResult);
    }
}
