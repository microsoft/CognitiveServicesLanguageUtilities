namespace  Microsoft.CustomTextCliUtils.Configs.Consts
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
    }
}