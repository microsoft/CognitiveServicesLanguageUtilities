// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Configs.Consts;
using Microsoft.IAPUtilities.Definitions.Exceptions.TextAnalytics;
using System;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Core.Services.TextAnalytics
{
    public class TextAnalyticsService : ITextAnalyticsService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly string _predictionLanguage;

        public TextAnalyticsService(string textAnalyticsEndpoint, string textAnalyticsKey, string predictionLanguage)
        {
            var credentials = new AzureKeyCredential(textAnalyticsKey);
            var endpoint = new Uri(textAnalyticsEndpoint);
            _textAnalyticsClient = new TextAnalyticsClient(endpoint, credentials);
            _predictionLanguage = predictionLanguage;
            TestConnection().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task TestConnection()
        {
            try
            {
                await _textAnalyticsClient.AnalyzeSentimentAsync("This hotel is great");
            }
            catch (Exception e)
            {
                throw new TextAnalyticsConnectionException(e.Message);
            }
        }

        private void VerifyCharLimit(string query)
        {

            if (query.Length > Constants.TextAnalyticsPredictionMaxCharLimit)
            {
                throw new TextAnalyticsExceededQueryLengthException();
            }
        }

        public async Task<DocumentSentiment> PredictSentimentAsync(string query, bool opinionMining)
        {
            // verify text analytics char limit
            VerifyCharLimit(query);
            var options = new AnalyzeSentimentOptions { IncludeOpinionMining = true };
            var response = await _textAnalyticsClient.AnalyzeSentimentAsync(query, language: _predictionLanguage, options);

            return response.Value;
        }
    }
}
