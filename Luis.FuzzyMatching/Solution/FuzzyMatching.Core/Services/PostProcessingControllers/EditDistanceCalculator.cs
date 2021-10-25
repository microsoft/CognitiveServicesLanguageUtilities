// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.PostProcessingControllers
{
    public static class EditDistanceCalculator
    {
        public static float CalculateEditDistance(string sentence1, string sentence2)
        {
            var dp = new int[sentence1.Length + 1, sentence2.Length + 1];

            for (int i = 0; i <= sentence1.Length; i++)
                dp[i, 0] = i;
            for (int i = 0; i <= sentence2.Length; i++)
                dp[0, i] = i;

            for (int i = 1; i <= sentence1.Length; i++)
            {
                for (int j = 1; j <= sentence2.Length; j++)
                {
                    if (sentence1[i - 1] == sentence2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {
                        var mn = Math.Min(dp[i, j - 1], dp[i - 1, j]);
                        mn = Math.Min(mn, dp[i - 1, j - 1]);
                        dp[i, j] = 1 + mn;
                    }
                }
            }

            var editDistance = dp[sentence1.Length, sentence2.Length];
            return NormalizeScore(editDistance, sentence1.Length, sentence2.Length);
        }

        private static float NormalizeScore(int editDistance, int firstSentenceSize, int secondSentenceSize)
        {
            var maxSize = Math.Max(firstSentenceSize, secondSentenceSize);
            return 1.0f - (editDistance / (float)maxSize);
        }
    }
}
