using Azure.AI.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Core.Services.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Tests.Configs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextCliUtils.Tests.IntegrationTests.Services.TextAnalytics
{
    public class TextAnalyticsPredictionServiceTest
    {
        public static TheoryData TextAnalyticsPredictTestData()
        {
            return new TheoryData<string, string, string, List<string>, CliException>
            {
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    new List<string> {"Ahmed's restaurant is great. Let's go eath there soon.", "Book me a flight to Cairo"},
                    null
                },
                {
                    "asdkjhfakds8asduf8audsf8as",
                    Secrets.TextAnalyticsEndpoint,
                    "en",
                    new List<string> {"Ahmed's restaurant is great. Let's go eath there soon.", "Book me a flight to Cairo"},
                    new TextAnalyticsConnectionException("")
                },
                {
                    Secrets.TextAnalyticsKey,
                    Secrets.TextAnalyticsEndpoint,
                    "xx",
                    new List<string> {"Ahmed's restaurant is great. Let's go eath there soon.", "Book me a flight to Cairo"},
                    new TextAnalyticsException(TextAnalyticsErrorCode.UnsupportedLanguageCode, "")
                }
            };
        }

        [Theory]
        [MemberData(nameof(TextAnalyticsPredictTestData))]
        public async Task PredictSentimentBatchAsyncTest(string key, string endpoint, string language, List<string> queries, CliException expectedException)
        {
            if (expectedException == null)
            {
                // act
                ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                var result = await predictionService.PredictSentimentBatchAsync(queries);

                // assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                foreach (AnalyzeSentimentResult r in result)
                {
                    Assert.NotNull(r.DocumentSentiment);
                    Assert.NotNull(r.DocumentSentiment.ConfidenceScores);
                    Assert.NotNull(r.DocumentSentiment.Sentences);
                    Assert.NotEmpty(r.DocumentSentiment.Sentences);
                }
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () =>
                {
                    ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                    await predictionService.PredictSentimentBatchAsync(queries);
                });
            }
        }

        [Theory]
        [MemberData(nameof(TextAnalyticsPredictTestData))]
        public async Task PredictNerBatchAsyncTest(string key, string endpoint, string language, List<string> queries, CliException expectedException)
        {
            if (expectedException == null)
            {
                // act
                ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                var result = await predictionService.PredictNerBatchAsync(queries);

                // assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                foreach (RecognizeEntitiesResult r in result)
                {
                    Assert.NotNull(r.Entities);
                    Assert.NotEmpty(r.Entities);
                    foreach (CategorizedEntity e in r.Entities)
                    {
                        Assert.NotNull(e.Text);
                    }
                }
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () =>
                {
                    ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                    await predictionService.PredictNerBatchAsync(queries);
                });
            }
        }

        [Theory]
        [MemberData(nameof(TextAnalyticsPredictTestData))]
        public async Task PredictKeyphraseBatchAsyncTest(string key, string endpoint, string language, List<string> queries, CliException expectedException)
        {
            if (expectedException == null)
            {
                // act
                ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                var result = await predictionService.PredictKeyphraseBatchAsync(queries);

                // assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                foreach (ExtractKeyPhrasesResult r in result)
                {
                    Assert.NotNull(r.KeyPhrases);
                    Assert.NotEmpty(r.KeyPhrases);
                }
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () =>
                {
                    ITextAnalyticsService predictionService = new TextAnalyticsService(key, endpoint, language);
                    await predictionService.PredictKeyphraseBatchAsync(queries);
                });
            }
        }
    }
}
