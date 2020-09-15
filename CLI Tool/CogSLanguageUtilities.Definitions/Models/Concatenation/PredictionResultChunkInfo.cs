using Azure.AI.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation
{
    public class PredictionResultChunkInfo
    {
        public ChunkInfo ChunkInfo;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomTextPredictionResponse CustomTextResponse;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AnalyzeSentimentResult SentimentResponse;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RecognizeEntitiesResult NerResponse;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExtractKeyPhrasesResult KeyphraseResponse;
    }
}
