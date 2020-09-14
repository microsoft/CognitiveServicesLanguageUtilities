using System;

namespace Microsoft.CustomTextCliUtils.Tests.Configs
{
    public class Secrets
    {
        public static readonly string MSReadCognitiveServicesEndPoint = Environment.GetEnvironmentVariable("MSREAD_COGNITIVE_SERVICES_ENDPOINT");
        public static readonly string MSReadCongnitiveServicesKey = Environment.GetEnvironmentVariable("MSREAD_COGNITIVE_SERVICES_KEY");
        public static readonly string StorageAccountConnectionString = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING");
        public static readonly string CustomTextKey = Environment.GetEnvironmentVariable("CUSTOM_TEXT_KEY");
        public static readonly string CustomTextEndpoint = Environment.GetEnvironmentVariable("CUSTOM_TEXT_ENDPOINT");
        public static readonly string CustomTextAppId = Environment.GetEnvironmentVariable("CUSTOM_TEXT_APP_ID");
        public static readonly string TextAnalyticsKey = Environment.GetEnvironmentVariable("TEXT_ANALYTICS_KEY");
        public static readonly string TextAnalyticsEndpoint = Environment.GetEnvironmentVariable("TEXT_ANALYTICS_ENDPOINT");
    }
}
