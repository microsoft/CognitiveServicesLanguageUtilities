// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models
{
    public class TokenMatchInfo
    {
        public string OriginalSentence { get; set; }
        public string TokenText { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public TokenMatchInfo()
        {
        }
        public TokenMatchInfo(TokenMatchInfo token)
        {
            TokenText = token.TokenText;
            StartIndex = token.StartIndex;
            EndIndex = token.EndIndex;
        }
        public bool Equals(TokenMatchInfo obj)
        {
            return TokenText == obj.TokenText
                    && StartIndex == obj.StartIndex
                    && EndIndex == obj.EndIndex;
        }
    }
}
