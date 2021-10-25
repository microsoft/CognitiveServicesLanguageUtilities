// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Enums;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Configs;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.E2ETests
{
    public class FuzzyMatchingE2ETests
    {
        public static TheoryData SingleMatchTestData()
        {
            var dataset = DatasetReader.ReadDatasetFromCSV(Constants.NewsHeadlinesDatasetLocation);

            var randomSentenceIndex = RandomGenerator.GetRandomIndex(dataset.Count);
            var sentenceToMatch = dataset[randomSentenceIndex];
            var expected = new MatchResult
            {
                DatabaseMatchInfo = new DatabaseMatchInfo()
                {
                    MatchText = sentenceToMatch,
                    MatchIndex = randomSentenceIndex
                }
            };

            return new TheoryData<List<string>, string, MatchResult>
            {
                {
                    dataset,
                    sentenceToMatch,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(SingleMatchTestData))]
        public void SingleMatchTest(List<string> dataset, string sentenceToMatch, MatchResult expected)
        {
            // initialize clients
            var preprocessorClient = new PreprocessorClient();
            var runtimeClient = new RuntimeClient();

            // process dataset
            var processedDataset = preprocessorClient.ProcessDataset(dataset);

            // runtime
            var resultList = runtimeClient.MatchEntities(processedDataset, dataset, sentenceToMatch, MatchingMethod.NoMatchIndices);
            var result = resultList[0];

            // assert
            Assert.Equal(result.DatabaseMatchInfo.MatchText, expected.DatabaseMatchInfo.MatchText);
            Assert.Equal(result.DatabaseMatchInfo.MatchIndex, expected.DatabaseMatchInfo.MatchIndex);

            // print result
            Console.WriteLine("sentence to match : {0}", sentenceToMatch);
            Console.WriteLine("Matched Sentence : {0}", result.DatabaseMatchInfo.MatchText);
            Console.WriteLine("Matched Sentence Score : {0}", result.SimilarityScore);
            Console.WriteLine("Matched Sentence Index : {0}", result.DatabaseMatchInfo.MatchIndex);
        }

        public static TheoryData MultipleMatchTestData()
        {
            var dataset = DatasetReader.ReadDatasetFromCSV(Constants.NewsHeadlinesDatasetLocation);

            var testDataPoints = 9;
            var testSentencesList = new List<string>();
            var expectedMatches = new List<MatchResult>();
            for (int i = 0; i < testDataPoints; i++)
            {
                // random datapoint
                var randomSentenceIndex = RandomGenerator.GetRandomIndex(dataset.Count);
                var sentenceToMatch = dataset[randomSentenceIndex];

                // expected outcome
                var expected = new MatchResult
                {
                    DatabaseMatchInfo = new DatabaseMatchInfo()
                    {
                        MatchText = dataset[randomSentenceIndex],
                        MatchIndex = randomSentenceIndex
                    }
                };

                // adding to lists
                testSentencesList.Add(sentenceToMatch);
                expectedMatches.Add(expected);
            }

            return new TheoryData<List<string>, List<string>, List<MatchResult>>
            {
                {
                    dataset,
                    testSentencesList,
                    expectedMatches
                }
            };
        }

        [Theory]
        [MemberData(nameof(MultipleMatchTestData))]
        public void MultipleMatchTest(List<string> dataset, List<string> testSentencesList, List<MatchResult> expected)
        {
            // initialize clients
            var preprocessorClient = new PreprocessorClient();
            var runtimeClient = new RuntimeClient();

            // process dataset
            var processedDataset = preprocessorClient.ProcessDataset(dataset);

            // exact matches
            var index = 0;
            for (; index < 3; index++)
            {
                var resultList = runtimeClient.MatchEntities(processedDataset, dataset, testSentencesList[index], MatchingMethod.NoMatchIndices, threshold: 0.8f);
                var result = resultList[0];
                Assert.Equal(1f, result.SimilarityScore, 4);
            }

            // altering sentence to decrease similarity scores
            testSentencesList[3] = testSentencesList[3].Remove(testSentencesList[3].Length - 2, 2).Insert(0, "Testing ") + " last word";
            testSentencesList[4] = testSentencesList[4].Insert(0, "The ");
            testSentencesList[5] = testSentencesList[5].Remove(testSentencesList[5].Length - 2, 2) + " nw";

            // close matches
            for (; index < 6; index++)
            {
                var resultList = runtimeClient.MatchEntities(processedDataset, dataset, testSentencesList[index], MatchingMethod.NoMatchIndices, threshold: 0.8f);
                var result = resultList[0];
                Assert.InRange(result.SimilarityScore, 0.5f, 1f);
            }

            // altering sentences heavily
            testSentencesList[6] = testSentencesList[6].Replace(testSentencesList[6][0], 'x').Insert(0, "Testing ");

            var halfLength = testSentencesList[7].Length / 2;
            testSentencesList[7] = testSentencesList[7].Insert(0, "The ").Insert(halfLength, "other words") + " more words";

            var thirdLength = testSentencesList[8].Length / 3;
            testSentencesList[8] = testSentencesList[8].Remove(0, thirdLength) + " added words.";

            // far matches
            for (; index < 9; index++)
            {
                var resultList = runtimeClient.MatchEntities(processedDataset, dataset, testSentencesList[index], MatchingMethod.NoMatchIndices, threshold: 0.5f);
                var result = resultList[0];
                Assert.InRange(result.SimilarityScore, 0f, 1f);
            }
        }
    }
}