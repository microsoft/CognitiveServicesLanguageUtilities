// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure.AI.TextAnalytics;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface ITextAnalyticsService
    {
        public Task<DocumentSentiment> PredictSentimentAsync(string query, bool opinionMining);
    }
}
