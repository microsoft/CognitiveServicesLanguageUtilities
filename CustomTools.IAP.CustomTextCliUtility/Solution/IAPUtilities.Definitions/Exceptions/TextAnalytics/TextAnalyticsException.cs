// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
namespace Microsoft.IAPUtilities.Definitions.Exceptions.TextAnalytics
{
    public class TextAnalyticsException : CliException
    {
        public TextAnalyticsException(string code, string message)
            : base(ConstructMessage(code, message))
        { }

        public static string ConstructMessage(string code, string message)
        {
            return $"Text Analytics failed with ErrorCode: {code} and message: {message}";
        }
    }
}
