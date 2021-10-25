// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers
{
    public static class NGramsController
    {
        /// <summary>
        /// Given a sentence, returns ngrams of that sentence
        /// example:
        ///     sentence = "department", ngramsLength = 3
        ///     result = ['dep', 'par', 'art', 'rtm', 'tme', 'men', 'ent']
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="ngramsLength"></param>
        /// <returns></returns>
        public static string[] GetSentenceNGrams(string sentence, int ngramsLength)
        {
            var result = new List<string>();

            for (var i = 0; i < sentence.Length - ngramsLength + 1; i++)
            {
                result.Add(sentence.Substring(i, ngramsLength));
            }

            return result.ToArray();
        }

        public static string[][] GetSentenceNGramsBatch(List<string> sentenceList, int ngramsLength)
        {
            return sentenceList.AsParallel().Select(sentence => GetSentenceNGrams(sentence, ngramsLength)).ToArray();
        }
        public static string[][] GetSentenceNGramsBatch(List<TokenMatchInfo> sentenceList, int ngramsLength)
        {
            return sentenceList.AsParallel().Select(sentence => GetSentenceNGrams(sentence.TokenText, ngramsLength)).ToArray();
        }
    }
}
