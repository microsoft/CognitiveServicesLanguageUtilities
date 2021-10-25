// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Enums
{
    public enum MatchingMethod
    {
        /// <summary>
        /// Get <see cref="MatchResult"/> without start and end indices for entities.
        /// </summary>
        NoMatchIndices,
        /// <summary>
        /// Get <see cref="MatchResult"/> with start and end indices for entities.
        /// Approach used: tokenize input sentence before mataching.
        /// </summary>
        PreprocessInputSentence,
        /// <summary>
        /// Get <see cref="MatchResult"/> with start and end indices for entities.
        /// Approach used: tokenize input sentence after mataching.
        /// </summary>
        PostprocessInputSentence

    }
}
