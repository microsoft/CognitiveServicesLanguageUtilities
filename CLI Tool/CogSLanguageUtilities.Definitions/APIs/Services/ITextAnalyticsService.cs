// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ITextAnalyticsService
    {
        public Task<List<AnalyzeSentimentResult>> PredictSentimentBatchAsync(List<string> queries);
        public Task<List<RecognizeEntitiesResult>> PredictNerBatchAsync(List<string> queries);
        public Task<List<ExtractKeyPhrasesResult>> PredictKeyphraseBatchAsync(List<string> queries);
    }
}
