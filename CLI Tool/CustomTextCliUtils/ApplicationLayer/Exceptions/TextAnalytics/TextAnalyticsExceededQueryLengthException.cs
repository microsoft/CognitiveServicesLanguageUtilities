namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.TextAnalytics
{
    public class TextAnalyticsExceededQueryLengthException : CliException
    {
        public TextAnalyticsExceededQueryLengthException()
            : base(ConstructMessage())
        { }

        public static string ConstructMessage()
        {
            return $"Text Analytics query length exceded. please consider modifying chunker char limit";
        }
    }
}
