// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers
{
    public static class NGramFrequencyController
    {
        public static Dictionary<string, int> GetNGramFrequency(string[] sentenceNGrams)
        {
            var result = new Dictionary<string, int>();
            foreach (var ngram in sentenceNGrams)
            {
                if (result.ContainsKey(ngram))
                {
                    result[ngram] += 1;
                }
                else
                {
                    result[ngram] = 1;
                }
            }
            return result;
        }

        public static Dictionary<string, int>[] GetNGramFrequencyBatch(string[][] sentenceDatasetNGrams)
        {
            return sentenceDatasetNGrams.AsParallel().Select(sentenceNGrams => GetNGramFrequency(sentenceNGrams)).ToArray();
        }

        public static async Task<Dictionary<string, int>> GetOverallNGramFrequency(string[][] sentenceListNGrams)
        {
            var result = new Dictionary<string, int>();
            var tasks = sentenceListNGrams.Select(async sentenceNGrams =>
            {
                var task = sentenceNGrams.Select(async ngram =>
                {
                    if (result.ContainsKey(ngram))
                    {
                        result[ngram] += 1;
                    }
                    else
                    {
                        result[ngram] = 1;
                    }
                });
                await Task.WhenAll(task);
            });
            await Task.WhenAll(tasks);

            return await Task.FromResult(result);
        }
    }
}