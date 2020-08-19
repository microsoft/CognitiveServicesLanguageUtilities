

namespace CustomTextCliUtils.Configs.Consts
{
    class Constants
    {
        public static readonly ConfigKeys ConfigKeys = new ConfigKeys();
        public static readonly string ConfigsFileLocalDirectory = @"";
        public static readonly string ConfigsFileName = "configs.json";
        public static readonly string[] ValidTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };

        public const string ToolName = "CustomTextUtils";
    }
    public enum ServiceName
    {
        Parser
    }
}
