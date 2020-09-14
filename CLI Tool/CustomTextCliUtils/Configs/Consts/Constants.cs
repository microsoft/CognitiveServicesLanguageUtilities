namespace Microsoft.CustomTextCliUtils.Configs.Consts
{
    public class Constants
    {
        // application
        public const string ToolName = "ctcu";
        // configs file
        public static readonly string ConfigsFileLocalDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string ConfigsFileName = "configs.json";
        // msread
        public static readonly string[] MsReadValidFileTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };
        // chunker
        public const double MaxLineLengthPrecentile = 0.95;
        public const double PercentageOfMaxLineLength = 0.98;
        // custom text
        public const int CustomTextPredictionMaxCharLimit = 25000;
        public const int MinAllowedCharLimit = 20;
        private const int CustomTextPredictionStatusTimeoutInSeconds = 20;
        public const int CustomTextPredictionStatusDelayInMillis = 500;
        public const int CustomTextPredictionStatusMaxIterations = CustomTextPredictionStatusTimeoutInSeconds * 1000 / CustomTextPredictionStatusDelayInMillis;
        // text analytics
        public const int TextAnalyticsPredictionMaxCharLimit = 5000;
        public const int TextAnaylticsApiCallDocumentLimit = 5;
    }
}