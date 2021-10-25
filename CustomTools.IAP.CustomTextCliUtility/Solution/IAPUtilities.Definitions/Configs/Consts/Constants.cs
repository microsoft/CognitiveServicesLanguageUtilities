// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.IAPUtilities.Definitions.Configs.Consts
{
    public class Constants
    {
        // tool name
        public const string ToolName = "iap";
        // configs file
        public static readonly string ConfigsFileLocalDirectory = ".";
        public static readonly string ConfigsFileName = "configs.json";
        // text analytics
        public const int TextAnalyticsPredictionMaxCharLimit = 5000;
        public const int TextAnaylticsApiCallDocumentLimit = 5;
        public const string TextAnalyticsLanguageCode = "en";
    }
}