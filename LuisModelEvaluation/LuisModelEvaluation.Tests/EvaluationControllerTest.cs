using Microsoft.LuisModelEvaluation.Controllers;
using Microsoft.LuisModelEvaluation.Exceptions;
using Microsoft.LuisModelEvaluation.Models.Input;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.LuisModelEvaluation.Tests
{
    public class EvaluationControllerTest
    {
        [Fact]
        public void EvaluateModelTest()
        {
            var entity = new Entity
            {
                Name = "testEntity",
                StartPosition = 1,
                EndPosition = 4,
                Children = null
            };
            var entities = new List<Entity>()
            {
                entity
            };
            var predictionData = new PredictionObject
            {
                Classification = "testClass",
                Entities = entities
            };
            var list = new List<TestingExample>
            {
                new TestingExample
                {
                    PredictedData = predictionData,
                    LabeledData = predictionData
                }
            };
            var evaluationController = new EvaluationController();
            var result = evaluationController.EvaluateModel(list);
            Assert.Equal(1, result.EntityModelsStats.Count);
            Assert.Equal(1, result.IntentModelsStats.Count);
            foreach (var entityStats in result.EntityModelsStats)
            {
                Assert.Equal(1.0, entityStats.Precision);
                Assert.Equal(1.0, entityStats.Recall);
                Assert.Equal(1.0, entityStats.FScore);
            }
            foreach (var intentStats in result.IntentModelsStats)
            {
                Assert.Equal(1.0, intentStats.Precision);
                Assert.Equal(1.0, intentStats.Recall);
                Assert.Equal(1.0, intentStats.FScore);
            }
            return;
        }

        public static TheoryData InvalidInputTestData()
        {
            return new TheoryData<IEnumerable<TestingExample>>
            {
                {
                    null
                },
                {
                    new List<TestingExample>
                    {
                        null
                    }
                },
                {
                    new List<TestingExample>
                    {
                        new TestingExample
                        {
                            PredictedData = null
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidInputTestData))]
        public void InvalidInputTest(IEnumerable<TestingExample> testData)
        {
            var evaluation = new EvaluationController();
            Assert.Throws<InvalidInputException>(() => evaluation.EvaluateModel(testData));
        }
    }
}
