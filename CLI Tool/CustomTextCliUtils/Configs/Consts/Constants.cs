namespace Microsoft.CustomTextCliUtils.Configs.Consts
{
    public class Constants
    {
        public static readonly string ConfigsFileLocalDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string ConfigsFileName = "configs.json";
        public static readonly string[] MsReadValidFileTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };
        public const string ToolName = "ctcu";
        public const double MaxLineLengthPrecentile = 0.95;
        public const double PercentageOfMaxLineLength = 0.98;
        public const int CustomTextPredictionMaxCharLimit = 25000;
        public const int MinAllowedCharLimit = 20;
        private const int CustomTextPredictionStatusTimeoutInSeconds = 20;
        public const int CustomTextPredictionStatusDelayInMillis = 500;
        public const int CustomTextPredictionStatusMaxIterations = CustomTextPredictionStatusTimeoutInSeconds * 1000 / CustomTextPredictionStatusDelayInMillis;
    }
}