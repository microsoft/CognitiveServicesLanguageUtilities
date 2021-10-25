// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Clients
{
    public interface IRuntimeClient
    {
        public List<MatchResult> MatchTokens(string sentence, ProcessedDataset processedDataset, List<string> Dataset, float similarityThreshold, int ngramsLength = 3);

        public List<MatchResult> MatchSentence(string sentence, ProcessedDataset processedDataset, List<string> dataset, float similarityThreshold, int ngramsLength);
    }
}
