// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models
{
    /// <summary>
    /// When matching "sentence" against a "database" (list<sentence>).
    /// </summary>
    public class MatchResult
    {
        /// <summary>
        /// Normalized similarity score (0: no match, 1: exact match).
        /// </summary>
        public float SimilarityScore { get; set; }
        /// <summary>
        /// The database sentence that is closet match to input sentence.
        /// </summary>
        public DatabaseMatchInfo DatabaseMatchInfo { get; set; }
        /// <summary>
        /// The token (SUBSTRING) from the input sentence
        /// which matches with the sentence from database.
        /// </summary>
        public TokenMatchInfo TokenMatchInfo;
    }
}
