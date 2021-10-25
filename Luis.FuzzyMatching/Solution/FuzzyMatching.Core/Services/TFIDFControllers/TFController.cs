// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers
{
    public static class TFController
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
        public static float[] CalculateTFVector(Dictionary<string, int> sentenceNGramsFrequencies, string[] allDataUniqueNGramsVector)
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

        public static float[][] CalculateTFVectorBatch(Dictionary<string, int>[] sentenceDatasetNGramFrequencies, string[] allDataUniqueNGramsVector)
        {
            return sentenceDatasetNGramFrequencies.AsParallel().Select(sentenceNGramsFrequencies => CalculateTFVector(sentenceNGramsFrequencies, allDataUniqueNGramsVector)).ToArray();
        }
    }
}
