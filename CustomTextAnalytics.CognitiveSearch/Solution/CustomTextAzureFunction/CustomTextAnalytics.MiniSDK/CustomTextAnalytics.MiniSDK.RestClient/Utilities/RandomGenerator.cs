using System;
using System.Linq;

namespace CustomTextAnalytics.MiniSDK.RestClient.Utilities
{
    internal class RandomGenerator
    {
        private static Random random = new Random();
        internal static string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal static string GenerateRandomId()
        {
            var id = Guid.NewGuid();
            return id.ToString();
        }
    }
}
