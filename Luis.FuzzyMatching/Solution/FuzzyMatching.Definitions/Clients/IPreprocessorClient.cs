// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Clients
{
    public interface IPreprocessorClient
    {
        public ProcessedDataset ProcessDataset(List<string> dataset, int ngramSize = default);
    }
}
