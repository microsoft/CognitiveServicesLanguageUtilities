// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Microsoft.IAPUtilities.Tests.Configs
{
    public class Secrets
    {
        // msread
        public static readonly string MSReadCognitiveServicesEndPoint = Environment.GetEnvironmentVariable("MSREAD_COGNITIVE_SERVICES_ENDPOINT");
        public static readonly string MSReadCongnitiveServicesKey = Environment.GetEnvironmentVariable("MSREAD_COGNITIVE_SERVICES_KEY");
        // blob storage
        public static readonly string StorageAccountConnectionString = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING");
        // custom text
        public static readonly string CustomTextKey = Environment.GetEnvironmentVariable("CUSTOM_TEXT_KEY");
        public static readonly string CustomTextEndpoint = Environment.GetEnvironmentVariable("CUSTOM_TEXT_ENDPOINT");
        public static readonly string CustomTextAppId = Environment.GetEnvironmentVariable("CUSTOM_TEXT_APP_ID");
        // text analytics
        public static readonly string TextAnalyticsKey = Environment.GetEnvironmentVariable("TEXT_ANALYTICS_KEY");
        public static readonly string TextAnalyticsEndpoint = Environment.GetEnvironmentVariable("TEXT_ANALYTICS_ENDPOINT");
    }
}
