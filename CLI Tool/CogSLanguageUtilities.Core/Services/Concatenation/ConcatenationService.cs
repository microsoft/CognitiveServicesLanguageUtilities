using Azure.AI.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;
using System;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Concatenation
{
    class ConcatenationService : IConcatenationService
    {
        public List<PredictionResultChunkInfo> ConcatPredictionResult(ChunkInfo[] chunkedText, List<CustomTextPredictionResponse> customTextResponse, List<AnalyzeSentimentResult> sentimentResponse, List<RecognizeEntitiesResult> nerResponse, List<ExtractKeyPhrasesResult> keyphraseResponse)
        {
            // TODO: return size of each response must be equal!
            var customTextCount = customTextResponse != null ? customTextResponse.Count : -1;
            var sentimentCount = sentimentResponse != null ? sentimentResponse.Count : -1;
            var nerCount = nerResponse != null ? nerResponse.Count : -1;
            var keyphraseCount = keyphraseResponse != null ? keyphraseResponse.Count : -1;
            var listCount = Math.Max(sentimentCount, Math.Max(nerCount, Math.Max(keyphraseCount, customTextCount)));

            // prepare
            var customTextArr = customTextResponse?.ToArray();
            var sentimentArr = sentimentResponse?.ToArray();
            var nerArr = nerResponse?.ToArray();
            var keyphraseArr = keyphraseResponse?.ToArray();

            var result = new List<PredictionResultChunkInfo>();
            for (int i = 0; i < listCount; i++)
            {
                var baseChunk = chunkedText[i];
                var currChunkInfo = new PredictionResultChunkInfo
                {
                    // chunk info
                    ChunkInfo = baseChunk,
                    // prediction info
                    SentimentResponse = i < sentimentCount ? sentimentArr[i] : null,
                    CustomTextResponse = i < customTextCount ? customTextArr[i] : null,
                    NerResponse = i < nerCount ? nerArr[i] : null,
                    KeyphraseResponse = i < keyphraseCount ? keyphraseArr[i] : null
                };
                result.Add(currChunkInfo);
            }
            return result;
        }

    }
}
