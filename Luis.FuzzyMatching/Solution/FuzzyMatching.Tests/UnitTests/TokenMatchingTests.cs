// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Enums;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Configs;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.UnitTests
{
    public class TokenMatchingTests
    {
        public static TheoryData TokenMatchingTestData(int testDataPoints = 1)
        {
            // prepare dataset and storage options
            var dataset = DatasetReader.ReadDatasetFromCSV(Constants.CompanyNamesDatasetLocation);

            // preparing datapoints for testing 
            var sentencesToMatchList = new List<string>();
            var expectedMatches = new List<MatchResult>();

            for (int i = 0; i < testDataPoints; i++)
            {
                //random datapoint
                var randomSentenceIndex = RandomGenerator.GetRandomIndex(dataset.Count);
                var sentenceToMatch = dataset[randomSentenceIndex];

                //constructing more complicated sentence that includes entity name
                var sentenceIncludingEntity = "Sentence that includes " + sentenceToMatch + " as an entity name";

                //expected outcome
                var expected = new MatchResult
                {
                    DatabaseMatchInfo = new DatabaseMatchInfo()
                    {
                        MatchText = dataset[randomSentenceIndex],
                        MatchIndex = randomSentenceIndex
                    }
                };

                //adding to lists
                sentencesToMatchList.Add(sentenceIncludingEntity);
                expectedMatches.Add(expected);
            }


            //returning TheoryData Objects
            if (testDataPoints == 1)
            {
                return new TheoryData<List<string>, string, MatchResult>
                {
                    {
                        dataset,
                        sentencesToMatchList[0],
                        expectedMatches[0]
                    }
                };
            }
            else
            {
                return new TheoryData<List<string>, List<string>, List<MatchResult>>
                {
                    {
                        dataset,
                        sentencesToMatchList,
                        expectedMatches
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(TokenMatchingTestData), parameters: 1)]
        public void TokenMatchingTest(List<string> dataset, string testSentence, MatchResult expected)
        {
            // initialize clients
            var preprocessorClient = new PreprocessorClient();
            var runtimeClient = new RuntimeClient();

            // process dataset
            var processedDataset = preprocessorClient.ProcessDataset(dataset);

            // runtime
            var resultList = runtimeClient.MatchEntities(processedDataset, dataset, testSentence, MatchingMethod.PreprocessInputSentence, threshold: 0.80f);
            var result = resultList.OrderByDescending(r => r.SimilarityScore).FirstOrDefault();

            // asserting that the right entity was correctly found
            // This is not a perfect test for this dataset since some entities are subsets of each other
            Assert.Equal(result.DatabaseMatchInfo.MatchText, expected.DatabaseMatchInfo.MatchText);
            Assert.Equal(result.DatabaseMatchInfo.MatchIndex, expected.DatabaseMatchInfo.MatchIndex);

            // print result
            Console.WriteLine("sentence to match : {0}", testSentence);
            Console.WriteLine("Matched Sentence : {0}", result.DatabaseMatchInfo.MatchText);
            Console.WriteLine("Matched Sentence Score : {0}", result.SimilarityScore);
            Console.WriteLine("Matched Sentence Index : {0}", result.DatabaseMatchInfo.MatchIndex);
        }
    }
}
