// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers
{
    public static class MatchFilter
    {
        public static List<MatchResult> FilterByThreshold(float[] similarityValues, List<string> dataset, float threshold, TokenMatchInfo token = default)
        {
            var matchResult = new List<MatchResult>();
            for (int i = 0; i < similarityValues.Length; i++)
            {
                if (similarityValues[i] >= threshold)
                {
                    matchResult.Add(new MatchResult
                    {
                        SimilarityScore = similarityValues[i],
                        TokenMatchInfo = token == default ? null : new TokenMatchInfo(token),
                        DatabaseMatchInfo = new DatabaseMatchInfo()
                        {
                            MatchText = dataset[i],
                            MatchIndex = i
                        }
                    });
                }
            }
            return matchResult;
        }

        public static List<MatchResult> FilterByThreshold(List<MatchResult> matchResult, float threshold)
        {
            return matchResult.FindAll(x => x.SimilarityScore >= threshold);
        }

        public static List<MatchResult> FilterByThresholdBatch(float[][] similarityMatrix, List<string> dataset, List<TokenMatchInfo> tokenList, float threshold)
        {
            var matchResult = new ConcurrentBag<MatchResult>();
            Parallel.For(0, similarityMatrix.Length, rowIndex =>
            {
                var result = FilterByThreshold(similarityMatrix[rowIndex], dataset, threshold, token: tokenList[rowIndex]);
                result.ForEach(val => matchResult.Add(val));
            });
            return matchResult.ToList();
        }
    }
}
