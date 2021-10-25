// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure.AI.TextAnalytics;
using Microsoft.IAPUtilities.Definitions.Enums.IAP;
using Microsoft.IAPUtilities.Definitions.Models.IAP;
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using System.Collections.Generic;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface IIAPResultGenerator
    {
        public ResultTranscript GenerateResult(IDictionary<long, CustomLuisResponse> luisPredictions, IDictionary<long, DocumentSentiment> textAnalyticsPredictions, ChannelType channel, string transcriptId);
    }
}