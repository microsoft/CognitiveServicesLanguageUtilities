using System;

namespace Microsoft.CustomTextCliUtils.Tests.Configs
{
    public class Secrets
    {
        public static readonly string MSReadCognitiveServiceEndPoint = Environment.GetEnvironmentVariable("MSREAD_COGNITIVE_SERVICES_ENDPOINT");
        public static readonly string MSReadCongnitiveServiceKey = Environment.GetEnvironmentVariable("MSREAD_COGNITIVE_SERVICES_KEY");
        public static readonly string StorageAccountConnectionString = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING");
    }
}
