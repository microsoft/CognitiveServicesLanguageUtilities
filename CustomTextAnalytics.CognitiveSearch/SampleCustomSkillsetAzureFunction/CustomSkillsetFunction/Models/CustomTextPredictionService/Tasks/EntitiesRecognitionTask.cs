using Azure.AI.TextAnalytics;
using CustomSkillsetFunction.Configs;
using CustomSkillsetFunction.ViewModels.Response;
using System.Collections.Generic;
using System.Linq;

namespace CustomSkillsetFunction.Models.CustomTextPredictionService.Tasks
{
    class EntitiesRecognitionTask : ITextAnalyticsTask
    {
        private string _projectName;
        private string _deploymentName;

        public EntitiesRecognitionTask(string projectName, string deploymentName)
        {
            _projectName = projectName;
            _deploymentName = deploymentName;
        }
        public void Inject(TextAnalyticsActions textAnalyticsActions)
        {
            textAnalyticsActions.RecognizeCustomEntitiesActions = new List<RecognizeCustomEntitiesAction>()
            {
                new RecognizeCustomEntitiesAction(_projectName, _deploymentName)
            };
        }

        public List<ResponseRecord> ExtractResult(AnalyzeActionsResult analyzeActionsResult)
        {
            var documents = ExtractDocumentEntityResult(analyzeActionsResult);

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
                    // group entities by category
                    var entitiesResult = GetEntities(document); // entity.category -> list(entity.text)

                    // add to result
                    var key = Constants.ResponseEntitiesKey;
                    documentRecord.Data.Add(key, entitiesResult);
                }

                return documentRecord;
            }).ToList();


        }

        private List<RecognizeEntitiesResult> ExtractDocumentEntityResult(AnalyzeActionsResult analyzeActionsResult)
        {
            /*
             * select first(), becuase we only submit one action
             * if we submitted multiple actions with different 'projectName', and 'deploymentName'
             * we can select more than first()
             */
            return analyzeActionsResult.RecognizeCustomEntitiesResults.First()
                            .DocumentsResults.ToList();
        }

        private Dictionary<string, List<string>> GetEntities(RecognizeEntitiesResult document)
        {
            var entitiesResult = new Dictionary<string, List<string>>();

            document.Entities
                .GroupBy(entities => (string)entities.Category)
                .ToList()
                .ForEach(group =>
                {
                    var category = group.Key;
                    var entities = group.Select(entity => entity.Text).ToList();
                    entitiesResult.Add(category, entities);
                });
            return entitiesResult;
        }
    }
}
