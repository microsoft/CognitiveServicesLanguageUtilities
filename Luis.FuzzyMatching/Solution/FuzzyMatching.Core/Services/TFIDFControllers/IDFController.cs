// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers
{
    public static class IDFController
    {
        /// <summary>
        /// calculates inverse frequence of word within all  documents 
        /// example:
        ///     for a total of 10,000,000 sentences with the ngram "dog" found 100 times
        ///     IDF = log( 10,000,000 / 100 ) = 4
        /// </summary>
        /// <param name="allDataUniqueNGramsVector"></param>
        /// <param name="overallDataNgramsFrequencies"></param>
        /// <param name="overallDataLength"></param>
        /// <returns></returns>
        /// 
        public static float[] CalculateIDFVector(string[] allDataUniqueNGramsVector, Dictionary<string, int> overallDataNgramsFrequencies, int overallDataLength)
        {
            var result = allDataUniqueNGramsVector.AsParallel().Select(ngram => (float)Math.Log(overallDataLength / (float)overallDataNgramsFrequencies[ngram])).ToArray();
            return result;
        }
    }
}
