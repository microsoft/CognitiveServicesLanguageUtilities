// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers.MatrixOperations;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.PostProcessingControllers;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Enums;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core
{
    public class RuntimeClient
    {
        public static readonly float PostProcessingThreshold = 0.8f;

        public List<MatchResult> MatchEntities(ProcessedDataset processedDataset, List<string> dataset, string inputSentence, MatchingMethod matchingMethod, float threshold = default, int ngramSize = default)
        {
            var matchingThreshold = threshold == default ? PostProcessingThreshold : threshold;

            switch (matchingMethod)
            {
                case MatchingMethod.NoMatchIndices:
                    return MatchEntitiesWithoutIndices(processedDataset, dataset, inputSentence, ngramSize);
                case MatchingMethod.PreprocessInputSentence:
                    return MatchEntitiesWithIndicesPreTokenizeApproach(processedDataset, dataset, inputSentence, matchingThreshold, ngramSize);
                case MatchingMethod.PostprocessInputSentence:
                    return MatchEntitiesWithIndicesPostTokenizeApproach(processedDataset, dataset, inputSentence, matchingThreshold, ngramSize);
                default:
                    throw new Exception("Matching method not supported!");
            }
        }

        private List<MatchResult> MatchEntitiesWithoutIndices(ProcessedDataset processedDataset, List<string> dataset, string inputSentence, int ngramSize)
        {
            // calculate input sentence TFIDF vector
            var inputSentenceTFIDFVector = TFIDFController.CalculateInputSentenceTFIDFVector(inputSentence, processedDataset, ngramSize);

            // calculate cosine similarity
            var cosineSimilarityValues = DotProductCalculator.GetDotProduct(inputSentenceTFIDFVector, processedDataset.TFIDFMatrix, matrixAbs: processedDataset.TFIDFMatrixAbsoluteValues);

            // filter result
            var tfidfThreshold = 0.4f;
            return MatchFilter.FilterByThreshold(cosineSimilarityValues, dataset, tfidfThreshold);
        }

        private List<MatchResult> MatchEntitiesWithIndicesPreTokenizeApproach(ProcessedDataset processedDataset, List<string> dataset, string inputSentence, float threshold, int ngramSize = 3)
        {
            // get all input sentence possible tokens
            var sentenceTokens = StringTokenizer.GetAllPossibleTokens(inputSentence, processedDataset.MaximumWordCount, ngramSize);

            // calculate tokens TFIDF matrix
            var inputTokensTFIDFMatrix = TFIDFController.CalculateInputSenenteceTokensTFIDFMatrix(sentenceTokens, processedDataset, ngramSize);

            // calculate tokens cosine similarity
            var similarityValuesMatrix = DotProductCalculator.GetDotProduct(inputTokensTFIDFMatrix, processedDataset.TFIDFMatrix, matrix2Abs: processedDataset.TFIDFMatrixAbsoluteValues);

            // filter results
            var tfidfThreshold = 0.5f;
            var tfidfMatches = MatchFilter.FilterByThresholdBatch(similarityValuesMatrix, dataset, sentenceTokens, tfidfThreshold);

            // post processing
            var updatedScoresMatches = PostprocessingController.UpdateMatchScores(tfidfMatches);
            return MatchFilter.FilterByThreshold(updatedScoresMatches, threshold);
        }

        private List<MatchResult> MatchEntitiesWithIndicesPostTokenizeApproach(ProcessedDataset processedDataset, List<string> dataset, string inputSentence, float threshold, int ngramSize = 3)
        {
            // initial match (reduce database)
            var initialMatchResult = MatchEntitiesWithoutIndices(processedDataset, dataset, inputSentence, ngramSize);
            if (initialMatchResult.Count == 0)
                return initialMatchResult;

            // get initial match sentences TFIDF values
            var matchingSentencesIndices = initialMatchResult.Select(m => m.DatabaseMatchInfo.MatchIndex).ToList();
            var initialMatchTFIDFMatrix = processedDataset.TFIDFMatrix.Where((rowValue, rowIndex) => matchingSentencesIndices.Contains(rowIndex)).ToArray();
            var initialMatchTFIDFAbsoluteValues = processedDataset.TFIDFMatrixAbsoluteValues.Where((rowValue, rowIndex) => matchingSentencesIndices.Contains(rowIndex)).ToArray();
            var initialMatchAsDataset = dataset.Where((rowValue, rowIndex) => matchingSentencesIndices.Contains(rowIndex)).ToList();

            // get all possible tokens of input sentence
            var sentenceTokens = StringTokenizer.GetAllPossibleTokens(inputSentence, processedDataset.MaximumWordCount, ngramSize);
            var inputTokensTFIDFMatrix = TFIDFController.CalculateInputSenenteceTokensTFIDFMatrix(sentenceTokens, processedDataset, ngramSize);

            // re-matching (with resolution)
            var similarityValuesMatrix = DotProductCalculator.GetDotProduct(inputTokensTFIDFMatrix, initialMatchTFIDFMatrix, matrix2Abs: initialMatchTFIDFAbsoluteValues);

            // re-filter
            var tfidfThreshold = 0.5f;
            var tfidfMatches = MatchFilter.FilterByThresholdBatch(similarityValuesMatrix, initialMatchAsDataset, sentenceTokens, tfidfThreshold);

            //post processing
            var updatedScoresMatches = PostprocessingController.UpdateMatchScores(tfidfMatches);
            return MatchFilter.FilterByThreshold(updatedScoresMatches, threshold);
        }
    }
}
