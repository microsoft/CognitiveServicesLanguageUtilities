// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models
{
    public class DatabaseMatchInfo
    {
        /// <summary>
        /// sentence index in the database.
        /// </summary>
        public int MatchIndex { get; set; }
        /// <summary>
        /// sentence actual text.
        /// </summary>
        public string MatchText { get; set; }
    }
}
