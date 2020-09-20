using Microsoft.LuisModelEvaluation.Controllers;
using Microsoft.LuisModelEvaluation.Exceptions;
using Microsoft.LuisModelEvaluation.Models.Input;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.LuisModelEvaluation.Tests
{
    public class EvaluationControllerTest
    {
        EvaluationController _evaluationController;

        public EvaluationControllerTest()
        {
            _evaluationController = new EvaluationController();
        }

        public static TheoryData EvaluateModelTestData()
        {
            var predictionData = new PredictionObject
            {
                Classification = new List<string> { "testClass" },
                Entities = new List<Entity>()
                {
                    new Entity
                    {
                        Name = "testEntity",
                        StartPosition = 1,
                        EndPosition = 20,
                        Children = new List<Entity>()
                        {
                            new Entity
                            {
                                Name = "testChildEntity",
                                StartPosition = 4,
                                EndPosition = 10,
                                Children = null
                            }
                        }
                    }
                }
            };
            var wrongPrediction = new PredictionObject
            {
                Classification = new List<string> { "wrongClass" },
                Entities = new List<Entity>()
                {
                    new Entity
                    {
                        Name = "wrongEntity",
                        StartPosition = 5,
                        EndPosition = 8,
                        Children = new List<Entity>()
                        {
                            new Entity
                            {
                                Name = "testWrongChildEntity",
                                StartPosition = 4,
                                EndPosition = 10,
                                Children = null
                            }
                        }
                    }
                }
            };
            var perfectScore = 1.0;
            var worstScore = 0.0;
            return new TheoryData<List<TestingExample>, double, int, int>
            {
                {
                    new List<TestingExample> {
                        new TestingExample
                        {
                            PredictedData = predictionData,
                            LabeledData = predictionData
                        }
                    },
                    perfectScore,
                    2,
                    1
                },
                {
                    new List<TestingExample> {
                        new TestingExample
                        {
                            PredictedData = wrongPrediction,
                            LabeledData = predictionData
                        }
                    },
                    worstScore,
                    4,
                    2
                }
            };
        }

        // Test perfect prediction and wrong prediction scenarios
        [Theory]
        [MemberData(nameof(EvaluateModelTestData))]
        public void EvaluateModelTest(List<TestingExample> testData, double score, int entityCount, int classCount)
        {
            var result = _evaluationController.EvaluateModel(testData);
            Assert.Equal(entityCount, result.EntityModelsStats.Count);
            Assert.Equal(classCount, result.IntentModelsStats.Count);
            foreach (var entityStats in result.EntityModelsStats)
            {
                Assert.Equal(score, entityStats.Precision);
                Assert.Equal(score, entityStats.Recall);
                Assert.Equal(score, entityStats.FScore);
            }
            foreach (var intentStats in result.IntentModelsStats)
            {
                Assert.Equal(score, intentStats.Precision);
                Assert.Equal(score, intentStats.Recall);
                Assert.Equal(score, intentStats.FScore);
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
