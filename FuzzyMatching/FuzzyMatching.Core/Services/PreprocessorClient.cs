using FuzzyMatching.Core.Utilities.FeatureMatrixArithmetics;
using FuzzyMatching.Core.Utilities.MatrixArithmetics;
using FuzzyMatching.Core.Utilities.MatrixOperations;
using FuzzyMatching.Definitions.Models;
using FuzzyMatching.Definitions.Services;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyMatching.Core.Services
{
    public class PreprocessorClient : IPreprocessorClient
    {
        public ProcessedDataset PreprocessDataset(List<string> dataset)
        {
            var calculatedFeaturesMatrices = new ProcessedDataset();
            // calculate ngrams for each sentence
            var ngramsLength = 3;
            var inputSentenceDatasetNGrams = NGramsCalculator.GetSentenceNGramsBatchAsync(dataset, ngramsLength);

            // calculate ngram frequencies
            var inputSentenceDatasetNGramFrequencies = FrequencyCalculator.GetNGramFrequencyBatchAsync(inputSentenceDatasetNGrams);
            var overallDataNgramFrequencies = FrequencyCalculator.GetOverallNGramFrequencyAsync(inputSentenceDatasetNGrams).GetAwaiter().GetResult();

            // get ngrams feature vector
            var allDataUniqueNGramsVector = overallDataNgramFrequencies.Keys.ToArray();

            // calculate TF
            var inputSentenceDatasetTFMatrix = TFCalculator.CalculateTFVectorBatchAsync(inputSentenceDatasetNGramFrequencies, allDataUniqueNGramsVector);

            // calculate IDF
            int overallDataLength = dataset.Count + 1;
            var overallDataIDFVector = IDFCalculator.CalculateIDFVector(allDataUniqueNGramsVector, overallDataNgramFrequencies, overallDataLength);

            // calculate TF-IDF
            var inputSentenceDatasetTFIDFMatrix = CellOperations.MultiplyVectorCellsBatch(inputSentenceDatasetTFMatrix, overallDataIDFVector);

            // get scalar values
            var inputSentenceDataseetAbsoluteValues = DotProductCalculator.CalculateVectorAbsoluteValueBatch(inputSentenceDatasetTFIDFMatrix);

            calculatedFeaturesMatrices.TFIDFMatrixAbsoluteValues = inputSentenceDataseetAbsoluteValues;
            calculatedFeaturesMatrices.TFIDFMatrix = inputSentenceDatasetTFIDFMatrix;
            calculatedFeaturesMatrices.IDFVector = overallDataIDFVector;
            calculatedFeaturesMatrices.UniqueNGramsVector = allDataUniqueNGramsVector;

            return calculatedFeaturesMatrices;
        }
    }
}
