// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.UnitTests
{
    public class StringTokenizerTests
    {
        public static TheoryData MaximumWordCountMockTestData()
        {
            var dataSet = new List<string>
            {
                "First Sentence test",
                "THis is the second sentence",
                "Im the sentence with the most words",
                "Last sentence"
            };

            // 7 is the expected value for the most words in a sentence
            return new TheoryData<List<string>, int>
            {
                {
                    dataSet,
                    7
                }
            };
        }
        [Theory]
        [MemberData(nameof(MaximumWordCountMockTestData))]
        public void MaxWordCountTest(List<string> testset, int expected)
        {
            var maxWordCount = StringTokenizer.FindMaxWordCount(testset);
            Assert.Equal(expected, maxWordCount);
        }
        public static TheoryData WordTokenizationTestData()
        {
            var dataset = new List<string>
            {
                "First Sentence test",
                "  space   test  ",
                "f s a",
                "oneword"
            };

            var expected = DataHelper.TestTokens1;

            // expected word tokens
            return new TheoryData<List<string>, List<List<TokenMatchInfo>>>
            {
                {
                    dataset,
                    expected
                }
            };
        }
        [Theory]
        [MemberData(nameof(WordTokenizationTestData))]
        public void WordTokenizationTest(List<string> testset, List<List<TokenMatchInfo>> expected)
        {
            // for each element in the testing set
            for (int i = 0; i < testset.Count; i++)
            {
                var wordTokens = StringTokenizer.TokenizeString(testset[i]);
                var areEqual = true;

                //manually check equality since XUnit has no Collection.Assert
                for (int j = 0; j < wordTokens.Count; j++)
                {
                    areEqual = areEqual && wordTokens[j].Equals(expected[i][j]);
                }

                Assert.True(areEqual && wordTokens.Count == expected[i].Count);
            }
        }
        public static TheoryData TokenGenerationTestData()
        {
            var dataset = DataHelper.TestTokens1;

            var expected = DataHelper.TestTokens1;

            // incomplete, we would like to create a list of the multi-word tokens expected to be produced
            return new TheoryData<List<List<TokenMatchInfo>>, List<List<TokenMatchInfo>>, int, int>
            {
                {
                    dataset,
                    expected,
                    3,
                    3
                }
            };
        }
        [Theory]
        [MemberData(nameof(TokenGenerationTestData))]
        public void TokenGenerationTest(List<List<TokenMatchInfo>> wordListTestset, List<List<TokenMatchInfo>> expected, int maxWordCount, int ngrams)
        {
            Console.WriteLine("======================");

            // for each element in the testing set
            foreach (var sentenceWords in wordListTestset)
            {
                var tokenList = StringTokenizer.GetAllPossibleTokens(sentenceWords, maxWordCount, ngrams);

                foreach (var token in tokenList)
                {
                    Console.WriteLine(token.TokenText);
                    Console.WriteLine(token.StartIndex);
                    Console.WriteLine(token.EndIndex);
                    Console.WriteLine("======================");
                }
            }
        }
    }
}
