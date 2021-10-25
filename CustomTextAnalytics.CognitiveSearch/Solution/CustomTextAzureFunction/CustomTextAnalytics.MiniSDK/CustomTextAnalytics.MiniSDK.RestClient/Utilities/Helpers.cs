using System.Linq;

namespace CustomTextAnalytics.MiniSDK.RestClient.Utilities
{
    internal class Helpers
    {
        internal static string ExtractJobIdFromLocationHeader(string operationLocationHeader)
        {
            return operationLocationHeader.Split("/").ToList().Last();
        }
    }
}
