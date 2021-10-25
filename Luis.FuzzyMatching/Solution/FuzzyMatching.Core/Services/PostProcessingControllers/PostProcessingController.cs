// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.PostProcessingControllers
{
    public static class PostprocessingController
    {
        public static List<MatchResult> UpdateMatchScores(List<MatchResult> matchResult)
        {
            foreach (var match in matchResult)
            {
                var tokenText = match.TokenMatchInfo.TokenText;
                var matchText = match.DatabaseMatchInfo.MatchText;

                var normalizedEditDistance = EditDistanceCalculator.CalculateEditDistance(tokenText, matchText);
                match.SimilarityScore = normalizedEditDistance;
            }
            return matchResult;
        }
    }
}