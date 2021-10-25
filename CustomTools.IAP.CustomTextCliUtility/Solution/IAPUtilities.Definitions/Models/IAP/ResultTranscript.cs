// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure.AI.TextAnalytics;
using Microsoft.IAPUtilities.Definitions.Enums.IAP;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.IAPUtilities.Definitions.Models.IAP
{
    public partial class ResultTranscript
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("conversation")]
        public List<Conversation> Conversation { get; set; }
    }

    public partial class Conversation
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("extractions")]
        public List<Extraction> Extractions { get; set; }

        public Sentiment Sentiment { get; set; }
    }

    public partial class Extraction
    {
        [JsonProperty("entityName")]
        public string EntityName { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<Extraction> Children { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("transcriptId")]
        public string TranscriptId { get; set; }

        [JsonProperty("channel")]
        public ChannelType Channel { get; set; }
    }

    public partial class Sentiment
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("confidenceScores")]
        public SentimentConfidenceScores ConfidenceScores { get; set; }

        [JsonProperty("opinions")]
        public List<Aspect> Opinions;
    }

    public partial class Opinion
    {
        [JsonProperty("text")]
        public string Text { get; set; }
       
        [JsonProperty("sentiment")]
        public string Sentiment { get; set; }
        
        [JsonProperty("isNegated")]
        public bool IsNegated { get; set; }
    }

    public class Aspect
    {
        [JsonProperty("aspectText")]
        public string AspectText { get; set; }

        [JsonProperty("sentiment")]
        public string Sentiment { get; set; }

        [JsonProperty("opinions")]
        public Opinion[] Opinions { get; set; }
    }

}
