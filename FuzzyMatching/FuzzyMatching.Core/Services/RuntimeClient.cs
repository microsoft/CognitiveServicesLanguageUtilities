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
        public List<MatchingResult> MatchSentence(string sentence, ProcessedDataset processedDataset, List<string> Dataset, float similarityThreshold, int ngramsLength = 3)
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


            if(similarityThreshold == default)
            {
                // get most matching one (match string, score, index)
                float maxValue = similarityValues.Max();
                int minIndex = similarityValues.ToList().IndexOf(maxValue);

                // return
                return new List<MatchingResult>
                {
                    new MatchingResult{
                        MatchingIndex = minIndex,
                        MatchingScore = maxValue,
                        ClosestSentence = Dataset[minIndex]
                    }
                };
            }
            else
            {
                //get any result with similarity > similarityThreshold

                List<MatchingResult> results = new List<MatchingResult>();

                //classic for loop to perform operation in one pass
                for(int i =0; i <similarityValues.Length; i++)
                {
                    if (similarityValues[i] >= similarityThreshold)
                    {
                        results.Add(
                            new MatchingResult {
                                MatchingIndex = i,
                                MatchingScore = similarityValues[i],
                                ClosestSentence = Dataset[i]
                            });
                    }
                }

                return results;
            }
        }
    }
}
