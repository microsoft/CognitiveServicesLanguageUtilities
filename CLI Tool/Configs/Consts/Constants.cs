

namespace CustomTextCliUtils.Configs.Consts
{
    class Constants
    {
        public static readonly ConfigKeys ConfigKeys = new ConfigKeys();
        public static readonly string ConfigsFileLocalDirectory = System.AppDomain.CurrentDomain.BaseDirectory; //@"";
        public static readonly string ConfigsFileName = "configs.json";
        public static readonly string[] ValidTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };

        public const string ToolName = "ctcu";
    }
    public enum ServiceName
    {
        Parser
    }
}
