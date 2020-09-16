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
                // Intent model stats aggregation
                evaluationService.AggregateIntentStats(testCase.LabeledData.Classification, testCase.PredictedData.Classification);

                // Prepare utterance stats
                var utteranceStats = new UtteranceStats
                {
                    UtteranceText = testCase.Text,
                    LabeledIntentNames = testCase.LabeledData.Classification,
                    PredictedIntentNames = testCase.PredictedData.Classification
                };

                // Populate False entities and Aggregate Entity MUC model stats
                evaluationService.PopulateUtteranceAndEntityStats(testCase.LabeledData.Entities, testCase.PredictedData.Entities, utteranceStats);
            }

            // Calculate precision, recall and fScore for Intent models
            var intentModelsStats = new List<ModelStats>(evaluationService.IntentsStats.Count);
            foreach (var intentConfusion in evaluationService.IntentsStats.Values)
            {
                intentModelsStats.Add(new ModelStats
                {
                    ModelName = intentConfusion.ModelName,
                    ModelType = intentConfusion.ModelType,
                    Precision = intentConfusion.CalculatePrecision(),
                    Recall = intentConfusion.CalculateRecall(),
                    FScore = intentConfusion.CalculateFScore(),
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
                IntentModelsStats = intentModelsStats,
                EntityModelsStats = entityModelsStats,
                UtterancesStats = evaluationService.UtterancesStats
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
                if (example.PredictedData.Classification.Count < 1 || example.LabeledData.Classification.Count < 1)
                {
                    throw new InvalidInputException("Classification");
                }
            }
        }
    }
}
