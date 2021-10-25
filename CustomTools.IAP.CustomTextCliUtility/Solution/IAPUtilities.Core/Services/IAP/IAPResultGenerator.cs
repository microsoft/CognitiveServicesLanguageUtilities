using Azure.AI.TextAnalytics;
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Enums.IAP;
using Microsoft.IAPUtilities.Definitions.Models.IAP;
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IAPUtilities.Core.Services.IAP
{
    public class IAPResultGenerator : IIAPResultGenerator
    {
        public ResultTranscript GenerateResult(
            IDictionary<long, CustomLuisResponse> luisPredictions,
            IDictionary<long, DocumentSentiment> textAnalyticsPredictions,
            ChannelType channel,
            string transcriptId)
        {

            var meta = new Meta
            {
                Channel = channel,
                TranscriptId = transcriptId
            };

            var conversations = luisPredictions.Where(prediction => prediction.Value.Entities != null)
                .Select(prediction =>
            {
                return new Conversation
                {
                    Text = prediction.Value.Query,
                    Timestamp = prediction.Key,
                    Extractions = prediction.Value.Entities,
                    Sentiment = MapSentimentResponse(textAnalyticsPredictions[prediction.Key])
                };
            });

            var sortedConversations = conversations.OrderBy(c => c.Timestamp).ToList();
            return new ResultTranscript
            {
                Conversation = sortedConversations,
                Meta = meta
            };
        }

        private Sentiment MapSentimentResponse(DocumentSentiment documentSentiment)
        {
            var aspects = new List<Aspect>();
            foreach (var sentence in documentSentiment.Sentences)
            {
                if (sentence.MinedOpinions.Count > 0)
                {
                    aspects.AddRange(sentence.MinedOpinions.Select(minedOpinion => new Aspect
                    {
                        AspectText = minedOpinion.Aspect.Text,
                        Sentiment = minedOpinion.Aspect.Sentiment.ToString(),
                        Opinions = minedOpinion.Opinions.Select(opinion => new Opinion
                        {
                            Sentiment = opinion.Sentiment.ToString(),
                            Text = opinion.Text,
                            IsNegated = opinion.IsNegated
                        }).ToArray()
                    }));
                }
            }
            return new Sentiment
            {
                Label = documentSentiment.Sentiment.ToString(),
                ConfidenceScores = documentSentiment.ConfidenceScores,
                Opinions = aspects
            };
        }
    }
}
