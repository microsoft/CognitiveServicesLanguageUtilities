// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget;
using Microsoft.CogSLanguageUtilities.Core.Services.Evaluation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation;
using Microsoft.LuisModelEvaluation.Models.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.IntegrationTests.ApplicationLayer.Services.Evaluation
{
    public class BatchTestingServiceTest
    {
        public static TheoryData RunBatchTestTestData()
        {
            var predicted = JsonConvert.DeserializeObject<PredictionObject>(File.ReadAllText(@"TestData\Evaluation\predicted.json"));
            var labeled = JsonConvert.DeserializeObject<PredictionObject>(File.ReadAllText(@"TestData\Evaluation\labeled.json"));
            var example = new TestingExample
            {
                LabeledData = labeled,
                PredictedData = predicted
            };
            var batchTestResponse = JsonConvert.DeserializeObject<BatchTestingOutput>(File.ReadAllText(@"TestData\Evaluation\batchTesting.json"));
            var examples = new List<TestingExample> { example };
            return new TheoryData<IEnumerable<TestingExample>, BatchTestingOutput>
            {
                {
                    examples,
                    batchTestResponse
                }
            };
        }

        [Theory]
        [MemberData(nameof(RunBatchTestTestData))]
        public void RunBatchTestTest(List<TestingExample> examples, BatchTestingOutput expectedResponse)
        {
            var service = new BatchTestingService();
            var batchTestResponse = service.RunBatchTest(examples, null, null);
            var mappedBatchTestResponse = BatchTestingOutputMapper.MapEvaluationOutput(batchTestResponse);
            Assert.Equal(expectedResponse, mappedBatchTestResponse, new BatchTestingOutputComparer());
        }

        public class BatchTestingOutputComparer : IEqualityComparer<BatchTestingOutput>
        {
            public bool Equals(BatchTestingOutput x, BatchTestingOutput y)
            {
                var xEntities = new List<BatchTestingModelStats>(x.EntityModelsStats);
                xEntities.Sort((x, y) => x.ModelName.CompareTo(y.ModelName));
                var xIntents = new List<BatchTestingModelStats>(x.ClassificationModelsStats);
                xIntents.Sort((x, y) => x.ModelName.CompareTo(y.ModelName));
                var yEntities = new List<BatchTestingModelStats>(y.EntityModelsStats);
                yEntities.Sort((x, y) => x.ModelName.CompareTo(y.ModelName));
                var yIntents = new List<BatchTestingModelStats>(y.ClassificationModelsStats);
                yEntities.Sort((x, y) => x.ModelName.CompareTo(y.ModelName));
                var entities = xEntities.Zip(yEntities, (x, y) => new { x, y });
                var intents = xIntents.Zip(yIntents, (x, y) => new { x, y });
                foreach (var item in intents)
                {
                    if (item.x.ModelName != item.y.ModelName || item.x.FScore != item.y.FScore || item.x.Precision != item.y.Precision)
                    {
                        return false;
                    }
                }
                foreach (var item in entities)
                {
                    if (item.x.ModelName != item.y.ModelName || item.x.EntityTextFScore != item.y.EntityTextFScore || item.x.EntityTypeFScore != item.y.EntityTypeFScore || item.x.FScore != item.y.FScore || item.x.Precision != item.y.Precision)
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(BatchTestingOutput obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
