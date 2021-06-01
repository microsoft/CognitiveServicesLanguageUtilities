using FuzzyMatching.Core.Utilities.FeatureMatrixArithmetics;
using FuzzyMatching.Core.Utilities.MatrixArithmetics;
using FuzzyMatching.Core.Utilities.MatrixOperations;
using FuzzyMatching.Definitions.Models;
using FuzzyMatching.Definitions.Services;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyMatching.Core.Services
{
    public class RuntimeClient : IRuntimeClient
    {
        public MatchingResult MatchSentence(string sentence, ProcessedDataset processedDataset, List<string> Dataset, int ngramsLength = 3)
        {
            // calculate ngrams for the sentence
            var inputSentenceNGrams = NGramsCalculator.GetSentenceNGramsAsync(sentence, ngramsLength);

            // calculate ngrams frequencies 
            var inputSentenceNGramFrequencies = FrequencyCalculator.GetNGramFrequencyAsync(inputSentenceNGrams);

            // calculate TF vector
            var inputSentenceTFVectorDataset = TFCalculator.CalculateTFVectorAsync(inputSentenceNGramFrequencies, processedDataset.UniqueNGramsVector);

            // calculate TF-IDF vector
            var inputSentenceTFIDFVectorDataset = CellOperations.MultiplyVectorCells(inputSentenceTFVectorDataset, processedDataset.IDFVector);

            // get absolute value
            var inputSentenceAbsoluteValue = DotProductCalculator.GetVectorAbsoluteValue(inputSentenceTFIDFVectorDataset);

            // calculate similarity
            var similarityValues = DotProductCalculator.CalculateDotProduct(inputSentenceTFIDFVectorDataset, inputSentenceAbsoluteValue, processedDataset.TFIDFMatrix, processedDataset.TFIDFMatrixAbsoluteValues);

            // get most matching one (match string, score, index)
            float minValue = similarityValues.Min();
            int minIndex = similarityValues.ToList().IndexOf(minValue);

            // return
            return new MatchingResult
            {
                MatchingIndex = minIndex,
                MatchingScore = minValue,
                ClosestSentence = Dataset[minIndex]
            };
        }
    }
}
