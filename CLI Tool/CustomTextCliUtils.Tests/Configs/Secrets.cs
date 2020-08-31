using System;

namespace Microsoft.CustomTextCliUtils.Tests.Configs
{
    public class Secrets
    {
        public static readonly string MSReadCognitiveServiceEndPoint = Environment.GetEnvironmentVariable("MSReadCognitiveServiceEndPoint");
        public static readonly string MSReadCongnitiveServiceKey = Environment.GetEnvironmentVariable("MSReadCongnitiveServiceKey");
        public static readonly string StorageAccountConnectionString = Environment.GetEnvironmentVariable("StorageAccountConnectionString");
    }
}
