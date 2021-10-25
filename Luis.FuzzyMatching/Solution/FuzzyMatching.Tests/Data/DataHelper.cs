// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Data
{
    public class DataHelper
    {
        public static List<List<TokenMatchInfo>> TestTokens1 => CreateListOfTokens();
        private static List<List<TokenMatchInfo>> CreateListOfTokens()
        {
            return new List<List<TokenMatchInfo>>
            {
                new List<TokenMatchInfo>
                {
                    new TokenMatchInfo
                    {
                        TokenText = "First",
                        StartIndex = 0,
                        EndIndex = 4
                    },
                    new TokenMatchInfo
                    {
                        TokenText = "Sentence",
                        StartIndex = 6,
                        EndIndex = 13
                    },
                    new TokenMatchInfo
                    {
                        TokenText= "test",
                        StartIndex =15,
                        EndIndex =18
                    }
                },
                new List<TokenMatchInfo>
                {
                    new TokenMatchInfo
                    {
                        TokenText = "space",
                        StartIndex = 2,
                        EndIndex = 6
                    },
                    new TokenMatchInfo
                    {
                        TokenText = "test",
                        StartIndex = 10,
                        EndIndex = 13
                    }
                },
                new List<TokenMatchInfo>
                {
                    new TokenMatchInfo
                    {
                        TokenText = "f",
                        StartIndex = 0,
                        EndIndex = 0
                    },
                    new TokenMatchInfo
                    {
                        TokenText = "s",
                        StartIndex = 2,
                        EndIndex = 2
                    },
                    new TokenMatchInfo
                    {
                        TokenText = "a",
                        StartIndex = 4,
                        EndIndex = 4
                    }
                },
                new List<TokenMatchInfo>
                {
                    new TokenMatchInfo
                    {
                        TokenText = "oneword",
                        StartIndex = 0,
                        EndIndex = 6
                    }
                }
            };
        }
    }
}
