// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Azure.AI.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Concatenation;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Result;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IConcatenationService
    {
        public List<PredictionResultChunkInfo> ConcatPredictionResult(ChunkInfo[] chunkedText, List<CustomTextPredictionResponse> customTextResponse, List<AnalyzeSentimentResult> sentimentResponse, List<RecognizeEntitiesResult> nerResponse, List<ExtractKeyPhrasesResult> keyphraseResponse);
    }
}
