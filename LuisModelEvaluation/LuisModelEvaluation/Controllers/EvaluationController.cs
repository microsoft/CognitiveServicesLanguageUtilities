using Microsoft.LuisModelEvaluation.Exceptions;
using Microsoft.LuisModelEvaluation.Models.Input;
using Microsoft.LuisModelEvaluation.Models.Result;
using Microsoft.LuisModelEvaluation.Services;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.LuisModelEvaluation.Controllers
{
    public class EvaluationController
    {
        public BatchTestResponse EvaluateModel(
            IEnumerable<TestingExample> testData,
            bool verbose = false,
            IEnumerable<Model> entities = null,
            IEnumerable<Model> classes = null)
        {
            ValidateInput(testData);

            // Intialize the evaluation service
            var evaluationService = new EvaluationService(entities, classes);

            foreach (var testCase in testData)
            {
                // classification model stats aggregation
                evaluationService.AggregateClassificationStats(new HashSet<string> ( testCase.LabeledData.Classification ), new HashSet<string> ( testCase.PredictedData.Classification ) );

                // Prepare query stats
                var queryStats = new QueryStats
                {
                    QueryText = testCase.Text,
                    LabeledClassNames = testCase.LabeledData.Classification,
                    PredictedClassNames = testCase.PredictedData.Classification
                };

                // Populate False entities and Aggregate Entity MUC model stats
                evaluationService.PopulateQueryAndEntityStats(testCase.LabeledData.Entities, testCase.PredictedData.Entities, queryStats);
            }

            // Calculate precision, recall and fScore for Classification models
            var classificationModelsStats = new List<ModelStats>(evaluationService.ClassificationStats.Count);
            foreach (var classificationConfusionMatrix in evaluationService.ClassificationStats.Values)
            {
                classificationModelsStats.Add(new ModelStats
                {
                    ModelName = classificationConfusionMatrix.ModelName,
                    ModelType = classificationConfusionMatrix.ModelType,
                    Precision = classificationConfusionMatrix.CalculatePrecision(),
                    Recall = classificationConfusionMatrix.CalculateRecall(),
                    FScore = classificationConfusionMatrix.CalculateFScore(),
                    EntityTextFScore = null,
                    EntityTypeFScore = null
                });
            }

            // Calculate precision, recall and fScore for Entity models
            var entityModelsStats = new List<ModelStats>(evaluationService.EntityStats.Count);
            foreach (var entitiesEvaluation in evaluationService.EntityStats.Values)
            {
                entityModelsStats.Add(new ModelStats
                {
                    ModelName = entitiesEvaluation.ModelName,
                    ModelType = entitiesEvaluation.ModelType,
                    Precision = entitiesEvaluation.CalculatePrecision(),
                    Recall = entitiesEvaluation.CalculateRecall(),
                    FScore = entitiesEvaluation.CalculateFScore(),
                    EntityTextFScore = verbose ? entitiesEvaluation.CalculateTextFScore() : (double?)null,
                    EntityTypeFScore = verbose ? entitiesEvaluation.CalculateTypeFScore() : (double?)null
                });
            }

            return new BatchTestResponse
            {
                ClassificationModelsStats = classificationModelsStats,
                EntityModelsStats = entityModelsStats,
                QueryStats = evaluationService.QueryStats
            };
        }

        private void ValidateInput(IEnumerable<TestingExample> testData)
        {
            if (testData == null || testData.Count() == 0)
            {
                throw new InvalidInputException("testData");
            }
            foreach (var example in testData)
            {
                if (example?.PredictedData == null || example?.LabeledData == null)
                {
                    throw new InvalidInputException("ActualData and LabeledData");
                }
                if (example.PredictedData.Classification == null || example.LabeledData.Classification == null)
                {
                    throw new InvalidInputException("Classification");
                }
                if (example.PredictedData.Entities == null || example.LabeledData.Entities == null)
                {
                    throw new InvalidInputException("Entities");
                }
            }
        }
    }
}
