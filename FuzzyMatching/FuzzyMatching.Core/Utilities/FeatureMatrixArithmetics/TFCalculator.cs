using System.Collections.Generic;
using System.Linq;

namespace FuzzyMatching.Core.Utilities.FeatureMatrixArithmetics
{
    public static class TFCalculator
    {
        /// <summary>
        /// calculate frequence of ngram within a sentence
        /// example:
        ///     for a sentence with 100 ngrams, the ngram "dog" found 3 times
        ///     TF = 3/100
        /// </summary>
        /// <param name="sentenceNGramsFrequencies"></param>
        /// <param name="allDataUniqueNGramsVector"></param>
        /// <returns></returns>
        public static float[] CalculateTFVectorAsync(Dictionary<string, int> sentenceNGramsFrequencies, string[] allDataUniqueNGramsVector)
        {
            var result = new float[allDataUniqueNGramsVector.Length];
            for (var i = 0; i < result.Length; i++)
            {
                var ngram = allDataUniqueNGramsVector[i];
                if (sentenceNGramsFrequencies.ContainsKey(ngram))
                {
                    result[i] = sentenceNGramsFrequencies[ngram];
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }

        public static float[][] CalculateTFVectorBatchAsync(Dictionary<string, int>[] sentenceDatasetNGramFrequencies, string[] allDataUniqueNGramsVector)
        {
            return sentenceDatasetNGramFrequencies.AsParallel().Select(sentenceNGramsFrequencies => CalculateTFVectorAsync(sentenceNGramsFrequencies, allDataUniqueNGramsVector)).ToArray();
        }
    }
}
