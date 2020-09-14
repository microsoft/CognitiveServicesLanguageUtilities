using Azure.AI.TextAnalytics;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.TextAnalytics
{
    public class TextAnalyticsPredictionChunkInfo
    {
        public ChunkInfo ChunkInfo;
        public AnalyzeSentimentResult SentimentResponse;
        public RecognizeEntitiesResult NerResponse;
        public ExtractKeyPhrasesResult KeyphraseResponse;
    }
}
