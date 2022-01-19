using Azure.AI.TextAnalytics;
using CustomSkillsetFunction.Configs;
using CustomSkillsetFunction.ViewModels.Response;
using System.Collections.Generic;
using System.Linq;

namespace CustomSkillsetFunction.Models.CustomTextPredictionService.Tasks
{
    class MultiClassificationTask : ITextAnalyticsTask
    {
        private string _projectName;
        private string _deploymentName;

        public MultiClassificationTask(string projectName, string deploymentName)
        {
            _projectName = projectName;
            _deploymentName = deploymentName;
        }
        public void Inject(TextAnalyticsActions textAnalyticsActions)
        {
            textAnalyticsActions.MultiCategoryClassifyActions = new List<MultiCategoryClassifyAction>()
            {
                new MultiCategoryClassifyAction(_projectName, _deploymentName)
            };
        }

        public List<ResponseRecord> ExtractResult(AnalyzeActionsResult analyzeActionsResult)
        {
            var documents = ExtractDocumentsClassificationResult(analyzeActionsResult);

            return documents.Select(document =>
            {
                var documentRecord = new ResponseRecord
                {
                    RecordId = document.Id
                };

                if (document.HasError)
                {
                    documentRecord.Errors.Add(new ErrorWarning() { Message = $"Error processing the request record : {document.Error.Message}" });
                }
                else
                {
                    var classifications = GetClassifications(document);
                    var key = Constants.ResponseMultiClassKey;
                    documentRecord.Data.Add(key, classifications);
                }
                return documentRecord;
            }).ToList();
        }
        private List<MultiCategoryClassifyResult> ExtractDocumentsClassificationResult(AnalyzeActionsResult analyzeActionsResult)
        {
            /*
             * select first(), becuase we only submit one action
             * if we submitted multiple actions with different 'projectName', and 'deploymentName'
             * we can select more than first()
             */
            return analyzeActionsResult.MultiCategoryClassifyResults.First().DocumentsResults.ToList();
        }
        private List<string> GetClassifications(MultiCategoryClassifyResult document)
        {
            return document.Classifications.Select(c => c.Category).ToList();
        }
    }
}
