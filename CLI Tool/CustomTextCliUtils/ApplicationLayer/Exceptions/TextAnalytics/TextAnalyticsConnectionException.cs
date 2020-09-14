using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.TextAnalytics
{
    public class TextAnalyticsConnectionException : CliException
    {
        public TextAnalyticsConnectionException(string message)
            : base(ConstructMessage(message))
        { }

        public static string ConstructMessage(string message)
        {
            return $"Connection to Azure Cognitive Services failed with message: \n{message} \nCheck Text Analytics configs " + 
                $"{ConfigKeys.TextAnalyticsAzureResourceKey} and {ConfigKeys.TextAnalyticsAzureResourceEndpoint}";
        }
    }
}
