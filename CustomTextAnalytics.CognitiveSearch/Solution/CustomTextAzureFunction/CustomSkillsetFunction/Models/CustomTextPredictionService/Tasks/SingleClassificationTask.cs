using Azure.AI.TextAnalytics;
using CustomSkillsetFunction.Configs;
using CustomSkillsetFunction.ViewModels.Response;
using System.Collections.Generic;
using System.Linq;

namespace CustomSkillsetFunction.Models.CustomTextPredictionService.Tasks
{
    class SingleClassificationTask : ITextAnalyticsTask
    {
        private string _projectName;
        private string _deploymentName;

        public SingleClassificationTask(string projectName, string deploymentName)
        {
            _projectName = projectName;
            _deploymentName = deploymentName;
        }
        public void Inject(TextAnalyticsActions textAnalyticsActions)
        {
            textAnalyticsActions.SingleCategoryClassifyActions = new List<SingleCategoryClassifyAction>()
            {
                new SingleCategoryClassifyAction(_projectName, _deploymentName)
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
                    var classification = GetClassification(document);
                    var key = Constants.ResponseSingleClassKey;
                    documentRecord.Data.Add(key, classification);
                }

                return documentRecord;
            }).ToList();
        }
        private List<SingleCategoryClassifyResult> ExtractDocumentsClassificationResult(AnalyzeActionsResult analyzeActionsResult)
        {
            /*
             * select first(), becuase we only submit one action
             * if we submitted multiple actions with different 'projectName', and 'deploymentName'
             * we can select more than first()
             */
            return analyzeActionsResult.SingleCategoryClassifyResults.First().DocumentsResults.ToList();
        }
        private string GetClassification(SingleCategoryClassifyResult document)
        {
            return document.Classification.Category;
        }
    }
}
