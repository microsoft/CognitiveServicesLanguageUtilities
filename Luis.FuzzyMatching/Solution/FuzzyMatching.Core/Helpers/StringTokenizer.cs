// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers
{
    public static class StringTokenizer
    {
        /// <summary>
        /// Given a sentence, returns the words of that sentence
        /// </summary>
        public static List<TokenMatchInfo> TokenizeString(string sentence)
        {
            var tokenList = new List<TokenMatchInfo>();

            //since we need the starting and ending index for each word, we need to tokenize manually
            //using string.split does not return indices
            int startIndex = 0;
            bool inWord = false;
            for (int i = 0; i < sentence.Length; i++)
            {
                if (inWord)
                {
                    //if we're already in the middle of a word
                    if (sentence[i] == ' ')
                    {
                        //space word terminator
                        inWord = false;
                        tokenList.Add(new TokenMatchInfo
                        {
                            TokenText = sentence.Substring(startIndex, i - startIndex),
                            EndIndex = i - 1,
                            StartIndex = startIndex
                        });
                    }

                    if (i == sentence.Length - 1)
                    {
                        //end of sentence terminator
                        tokenList.Add(new TokenMatchInfo
                        {
                            TokenText = sentence.Substring(startIndex, i - startIndex + 1),
                            EndIndex = i,
                            StartIndex = startIndex
                        });
                    }
                }
                else
                {
                    if (sentence[i] != ' ')
                    {
                        //start of new word
                        inWord = true;
                        startIndex = i;

                        if (i == sentence.Length - 1)
                        {
                            //end of sentence terminator
                            tokenList.Add(new TokenMatchInfo
                            {
                                TokenText = sentence.Substring(startIndex, i - startIndex + 1),
                                EndIndex = i,
                                StartIndex = startIndex
                            });
                        }
                    }
                }
            }

            return tokenList;
        }

        public static List<TokenMatchInfo> GetAllPossibleTokens(string sentence, int maximumWordCount, int ngramSize)
        {
            // break sentence down to words
            var sentenceWords = TokenizeString(sentence);

            // turn words into tokens of length 1 -> processedDataset.MaximumWordCount
            return GetAllPossibleTokens(sentenceWords, maximumWordCount, ngramSize);
        }

        /// <summary>
        /// Finds the maximum number of words in any singular datapoint within a given dataset.
        /// example:
        ///     Dataset = "hello this is a test", "hi another test", "im a datapoint!"
        ///     returns: 5
        /// </summary>
        public static int FindMaxWordCount(List<string> dataset)
        {
            int maximumWordCount = 0;

            foreach (var datapoint in dataset)
            {
                maximumWordCount = Math.Max(maximumWordCount, datapoint.Split(' ').Length);
            }

            return maximumWordCount;
        }

        /// <summary>
        /// Creates all the multi-word tokens, up to tokens containing maximumWordCount words
        /// </summary>
        public static List<TokenMatchInfo> GetAllPossibleTokens(List<TokenMatchInfo> tokenList, int maximumWordCount, int ngramsLength)
        {
            var tokenListResult = new List<TokenMatchInfo>();

            //adding single word tokens
            foreach (var token in tokenList)
            {
                AddToList(tokenListResult, token, ngramsLength);
            }

            //adding multi word tokens
            //looping on first word of this current token
            for (int firstWordIndex = 0; firstWordIndex < tokenList.Count; firstWordIndex++)
            {
                //the string that will be built upon as we add more words
                var currentWord = tokenList[firstWordIndex].TokenText;
                var startIndex = tokenList[firstWordIndex].StartIndex;

                //loop until we have either included as many words as maxWordCount, or we've reached the last word of the sentence
                for (int tokenSize = 1; tokenSize < maximumWordCount && tokenSize + firstWordIndex < tokenList.Count; tokenSize++)
                {
                    //how many words forward will we include this time? forward word 

                    //adding last word to the string
                    currentWord += " " + tokenList[firstWordIndex + tokenSize].TokenText;

                    //create new token object
                    var newToken = new TokenMatchInfo
                    {
                        TokenText = currentWord,
                        StartIndex = startIndex,
                        EndIndex = tokenList[firstWordIndex + tokenSize].EndIndex
                    };

                    AddToList(tokenListResult, newToken, ngramsLength);
                }

            }
            return tokenListResult;
        }

        private static void AddToList(List<TokenMatchInfo> result, TokenMatchInfo token, int ngramsLength)
        {
            if (token.TokenText.Length >= ngramsLength)
            {
                result.Add(token);
            }
        }
    }
}
