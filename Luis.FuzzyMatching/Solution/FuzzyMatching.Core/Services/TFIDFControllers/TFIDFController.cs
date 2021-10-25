// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers.MatrixOperations;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Configs;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers
{
    class TFIDFController
    {
        private static readonly int NGramSize = Constants.NGramSize;

        public static void CalculateDatasetTFIDFValues(List<string> dataset, out string[] uniqueNGramsVector, out float[] datasetIDFVector, out float[][] datasetTFIDFMatrix, int ngramSize = default)
        {
            // calculate ngrams for each sentence
            var ngSize = ngramSize == default ? NGramSize : ngramSize;
            var datasetNGrams = NGramsController.GetSentenceNGramsBatch(dataset, ngSize);

            // calculate ngram frequencies
            var ngramFrequencies = NGramFrequencyController.GetNGramFrequencyBatch(datasetNGrams);
            var overallNgramFrequencies = NGramFrequencyController.GetOverallNGramFrequency(datasetNGrams).GetAwaiter().GetResult();

            // get ngrams feature vector
            uniqueNGramsVector = overallNgramFrequencies.Keys.ToArray();

            // calculate TF
            var datasetTFMatrix = TFController.CalculateTFVectorBatch(ngramFrequencies, uniqueNGramsVector);

            // calculate IDF
            int dataLength = dataset.Count + 1;
            datasetIDFVector = IDFController.CalculateIDFVector(uniqueNGramsVector, overallNgramFrequencies, dataLength);

            // calculate TF-IDF
            datasetTFIDFMatrix = MultiplicationCalculator.MultiplyVectorsByCellBatch(datasetTFMatrix, datasetIDFVector);
        }

        public static float[] CalculateInputSentenceTFIDFVector(string sentence, ProcessedDataset processedDataset, int ngramSize)
        {
            // calculate ngrams for the sentence
            var ngSize = ngramSize == default ? NGramSize : ngramSize;
            var sentenceNGrams = NGramsController.GetSentenceNGrams(sentence, ngSize);

            // calculate ngrams frequencies 
            var sentenceNGramFrequencies = NGramFrequencyController.GetNGramFrequency(sentenceNGrams);

            // calculate TF vector
            var sentenceTFVectorDataset = TFController.CalculateTFVector(sentenceNGramFrequencies, processedDataset.UniqueNGramsVector);

            // calculate TF-IDF vector
            return MultiplicationCalculator.MultiplyVectorsByCell(sentenceTFVectorDataset, processedDataset.IDFVector);
        }

        public static float[][] CalculateInputSenenteceTokensTFIDFMatrix(List<TokenMatchInfo> sentenceTokens, ProcessedDataset processedDataset, int ngramSize)
        {
            // CAN BE MADE MORE EFFICIENT SINCE SOME TOKENS ARE SUBTOKENS OF OTHERS
            // TODO: Implement more efficient solution that makes use of Sub-tokens
            // calculate ngrams for tokens 
            var ngSize = ngramSize == default ? NGramSize : ngramSize;
            var tokensNgrams = NGramsController.GetSentenceNGramsBatch(sentenceTokens, ngSize);

            // calculate N-Gram Frequencies
            var inputTokensNGramFrequencies = NGramFrequencyController.GetNGramFrequencyBatch(tokensNgrams);

            // calculate TF Matrix
            var inputTokensTFMaxtrixDataset = TFController.CalculateTFVectorBatch(inputTokensNGramFrequencies, processedDataset.UniqueNGramsVector);

            // calculate TF-IDF Matrix
            var inputTokensTFIDFMatrixDataset = MultiplicationCalculator.MultiplyVectorsByCellBatch(inputTokensTFMaxtrixDataset, processedDataset.IDFVector);
            return inputTokensTFIDFMatrixDataset;
        }
    }
}
