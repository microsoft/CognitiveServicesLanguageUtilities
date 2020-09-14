using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics
{
    public interface ITextAnalyticsPredictionService
    {
        public Task<List<AnalyzeSentimentResult>> PredictSentimentBatchAsync(List<string> queries);
        public Task<List<RecognizeEntitiesResult>> PredictNerBatchAsync(List<string> queries);
        public Task<List<ExtractKeyPhrasesResult>> PredictKeyphraseBatchAsync(List<string> queries);
    }
}
