// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Enums;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Configs;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.PerformanceTests
{
    public class FuzzyMatchingPerfomanceTests
    {
        public static TheoryData FuzzyMatchingClientPerfomanceTestData()
        {
            var dataset = DatasetReader.ReadDatasetFromCSV(Constants.NewsHeadlinesDatasetLocation);

            int[] testDataSizes = new int[6] { 10, 100, 1000, 10000, 25000, 50000 };

            var sentenceToMatch = "take record";

            return new TheoryData<List<string>, int[], string>
            {
                {
                    dataset,
                    testDataSizes,
                    sentenceToMatch
                }
            };
        }
        [Theory]
        [MemberData(nameof(FuzzyMatchingClientPerfomanceTestData))]
        public void FuzzyMatchingClientPerfomanceTest(List<string> dataset, int[] testDataSizes, string testSentence)
        {
            foreach (var dataSize in testDataSizes)
            {
                // read dataset
                var tmp = dataset.Take(dataSize).ToList();

                // initialize clients
                var preprocessorClient = new PreprocessorClient();
                var runtimeClient = new RuntimeClient();

                // process dataset
                var processedDataset = preprocessorClient.ProcessDataset(tmp);

                // performance test
                var start = DateTime.Now;

                runtimeClient.MatchEntities(processedDataset, dataset, testSentence, MatchingMethod.NoMatchIndices);

                var end = DateTime.Now;
                var ts = end - start;

                // print time
                Console.WriteLine("Elapsed Time for the program with size {0} is {1} s", dataSize, ts.TotalSeconds);
            }
        }
    }
}
