using Azure;
using System;
using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.Utilities;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics
{
    public class TextAnalyticsPredictionService : ITextAnalyticsPredictionService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly string _predictionLanguage;

        public TextAnalyticsPredictionService(string textAnalyticsKey, string textAnalyticsEndpoint, string predictionLanguage)
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
        public async Task<List<AnalyzeSentimentResult>> PredictSentimentBatchAsync(List<string> queries)
        {
            // verify text analytics char limit
            VerifyCharLimit(queries);
            // paginate throw chunks to abide by "DocumentCountLimit" of the api call
            var listPaginator = new Paginator<string>(queries, Constants.TextAnaylticsApiCallDocumentLimit);
            var result = new List<AnalyzeSentimentResult>();
            while (listPaginator.HasNext()) {
                var subList = (listPaginator.GetNextPage()).ToList();
                var response = await _textAnalyticsClient.AnalyzeSentimentBatchAsync(subList, language: _predictionLanguage);
                HandleError(response.Value);
                result.AddRange(response.Value);
            }
            return result;
        }
        public async Task<List<RecognizeEntitiesResult>> PredictNerBatchAsync(List<string> queries)
        {
            // verify text analytics char limit
            VerifyCharLimit(queries);
            // paginate throw chunks to abide by "DocumentCountLimit" of the api call
            var listPaginator = new Paginator<string>(queries, Constants.TextAnaylticsApiCallDocumentLimit);
            var result = new List<RecognizeEntitiesResult>();
            while (listPaginator.HasNext())
            {
                var subList = (listPaginator.GetNextPage()).ToList();
                var response = await _textAnalyticsClient.RecognizeEntitiesBatchAsync(subList, language: _predictionLanguage);
                HandleError(response.Value);
                result.AddRange(response.Value);
            }
            return result;
        }

        public async Task<List<ExtractKeyPhrasesResult>> PredictKeyphraseBatchAsync(List<string> queries)
        {
            // verify text analytics char limit
            VerifyCharLimit(queries);
            // paginate throw chunks to abide by "DocumentCountLimit" of the api call
            var listPaginator = new Paginator<string>(queries, Constants.TextAnaylticsApiCallDocumentLimit);
            var result = new List<ExtractKeyPhrasesResult>();
            while (listPaginator.HasNext())
            {
                var subList = (listPaginator.GetNextPage()).ToList();
                var response = await _textAnalyticsClient.ExtractKeyPhrasesBatchAsync(subList, language: _predictionLanguage);
                HandleError(response.Value);
                result.AddRange(response.Value);
            }
            return result;
        }
        private void VerifyCharLimit(List<string> queries)
        {
            queries.ForEach(q =>
            {
                if (q.Length > Constants.TextAnalyticsPredictionMaxCharLimit)
                {
                    throw new TextAnalyticsExceededQueryLengthException();
                }
            });
        }
        private void HandleError(IEnumerable<TextAnalyticsResult> results)
        {
            foreach (TextAnalyticsResult r in results)
            {
                if (r.HasError)
                {
                    throw new TextAnalyticsException(r.Error.ErrorCode.ToString(), r.Error.Message);
                }
            }
        }
    }
}
