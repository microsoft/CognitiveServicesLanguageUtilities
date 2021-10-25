// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure.AI.TextAnalytics;
using Microsoft.IAPUtilities.Core.Services.TextAnalytics;
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Exceptions;
using Microsoft.IAPUtilities.Definitions.Exceptions.TextAnalytics;
using Microsoft.IAPUtilities.Tests.Configs;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextCliUtils.Tests.IntegrationTests.Services.TextAnalytics
{
    public class TextAnalyticsPredictionServiceTest
    {
        public static TheoryData TextAnalyticsPredictTestData()
        {
            return new TheoryData<string, string, string, string, CliException>
            {
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    "Ahmed's restaurant is great. Let's go eat there soon.",
                    null
                },
                {
                    "asdkjhfakds8asduf8audsf8as",
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    "Ahmed's restaurant is great. Let's go eat there soon.",
                    new TextAnalyticsConnectionException("")
                },
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "xx",
                    "Ahmed's restaurant is great. Let's go eat there soon.",
                    new TextAnalyticsException(TextAnalyticsErrorCode.UnsupportedLanguageCode, "")
                }
            };
        }

        [Theory]
        [MemberData(nameof(TextAnalyticsPredictTestData))]
        public async Task PredictSentimentBatchAsyncTest(string key, string endpoint, string language, string query, CliException expectedException)
        {
            if (expectedException == null)
            {
                // act
                ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                var result = await predictionService.PredictSentimentAsync(query);

                // assert
                Assert.NotNull(result);

                Assert.NotNull(result.ConfidenceScores);
                Assert.NotNull(result.Sentences);
                Assert.NotEmpty(result.Sentences);
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () =>
                {
                    ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                    await predictionService.PredictSentimentAsync(query);
                });
            }
        }
    }
}
