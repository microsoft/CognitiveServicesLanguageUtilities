using System.Collections.Generic;
using System.Linq;

namespace FuzzyMatching.Core.Utilities.FeatureMatrixArithmetics
{
    public static class NGramsCalculator
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
        public static string[] GetSentenceNGramsAsync(string sentence, int ngramsLength = 3)
        {
            var result = new List<string>();

            for (var i = 0; i < sentence.Length - ngramsLength + 1; i++)
            {
                result.Add(sentence.Substring(i, ngramsLength));
            }

            return result.ToArray();
        }

        public static string[][] GetSentenceNGramsBatchAsync(List<string> sentenceList, int ngramsLength = 3)
        {
            return sentenceList.AsParallel().Select(sentence => GetSentenceNGramsAsync(sentence, ngramsLength)).ToArray();
        }
    }
}
