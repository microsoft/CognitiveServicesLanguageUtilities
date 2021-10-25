// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Enums;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.E2ETests
{
    public class MatchIndexResolutionTests
    {
        public PreprocessorClient _preprocessorClient;
        public RuntimeClient _runtimeClient;

        public MatchIndexResolutionTests()
        {
            _preprocessorClient = new PreprocessorClient();
            _runtimeClient = new RuntimeClient();
        }
        public static TheoryData MatchIndexResolutionTestData()
        {
            // WARNING: PLEASE DON'T TAMPER WITH THE VALUES
            var inputSentence = "i want to book a flight ticket from cairoo to lonndon next week";
            var dataset = new List<string>
            {
                "cairo",
                "london",
                "new york",
                "berlin",
                "miami",
                "los angeles",
                "paris",
                "madrid"
            };

            var expected = new List<MatchResult>
            {
                new MatchResult()
                {
                    DatabaseMatchInfo = new DatabaseMatchInfo(){ MatchText = dataset[0] },
                    TokenMatchInfo = new TokenMatchInfo(){ StartIndex = 36, EndIndex = 41}
                },
                new MatchResult()
                {
                    DatabaseMatchInfo = new DatabaseMatchInfo(){ MatchText = dataset[1] },
                    TokenMatchInfo = new TokenMatchInfo(){ StartIndex = 46, EndIndex = 52}
                }
            };

            return new TheoryData<string, List<string>, List<MatchResult>>
            {
                {
                    inputSentence,
                    dataset,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(MatchIndexResolutionTestData))]
        public void MatchIndexResolutionTest(string inputSentence, List<string> dataset, List<MatchResult> expected)
        {
            // process dataset
            var processedDataset = _preprocessorClient.ProcessDataset(dataset);

            // matching
            var threshold = 0.8f;
            var matchList = _runtimeClient.MatchEntities(processedDataset, dataset, inputSentence, MatchingMethod.PostprocessInputSentence, threshold: threshold);

            // assert
            var invalidTestFlag = true;
            matchList.ForEach(m =>
            {
                var matchText = m.DatabaseMatchInfo.MatchText;
                expected.ForEach(e =>
                {
                    var expectedText = e.DatabaseMatchInfo.MatchText;
                    if (string.Equals(matchText, expectedText))
                    {
                        invalidTestFlag = false;
                        Assert.Equal(m.TokenMatchInfo.StartIndex, e.TokenMatchInfo.StartIndex);
                        Assert.Equal(m.TokenMatchInfo.EndIndex, e.TokenMatchInfo.EndIndex);
                    }
                });
            });

            if (invalidTestFlag)
                throw new Exception("No result matched with any of the expected values!");
        }

    }
}