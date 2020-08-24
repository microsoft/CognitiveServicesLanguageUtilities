namespace CustomTextCliUtils.Configs.Consts
{
    class Constants
    {
        public static readonly string ConfigsFileLocalDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string ConfigsFileName = "configs.json";
        public static readonly string[] ValidTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };
        public const string ToolName = "ctcu";
        public const double PercentageOfMaxLineLength = 0.98;
        public const int CustomTextPredictionMaxCharLimit = 23000;
    }
}