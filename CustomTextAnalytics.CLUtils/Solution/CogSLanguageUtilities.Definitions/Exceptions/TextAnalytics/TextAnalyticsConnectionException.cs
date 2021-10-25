// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.IAPUtilities.Definitions.Exceptions.TextAnalytics
{
    public class TextAnalyticsConnectionException : CliException
    {
        public TextAnalyticsConnectionException(string message)
            : base(ConstructMessage(message))
        { }

        public static string ConstructMessage(string message)
        {
            return $"Connection to Azure Cognitive Services failed with message: \n{message} \nCheck Text Analytics configs ";
        }
    }
}
