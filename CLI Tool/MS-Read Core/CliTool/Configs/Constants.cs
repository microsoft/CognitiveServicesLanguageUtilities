using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace CliTool.Configs
{
    class Constants
    {
        public static readonly ConfigKeys ConfigKeys = new ConfigKeys();
        public static readonly string ConfigsFileDir = @"C:\Users\a-noyass\Desktop\LUIS\Cognitive-Custom_text_Utilities\CLI Tool\MS-Read Core\CliTool\configs.json";
        public static readonly string[] ValidTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };
    }
    public enum ServiceName
    {
        Parser
    }
}
