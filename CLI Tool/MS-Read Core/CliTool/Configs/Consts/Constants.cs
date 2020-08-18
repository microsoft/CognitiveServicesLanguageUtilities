using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace CliTool.Configs.Consts
{
    class Constants
    {
        public static readonly ConfigKeys ConfigKeys = new ConfigKeys();
        public static readonly string ConfigsFileLocalDirectory = "";
        public static readonly string ConfigsFileName = "configs.json";
        public static readonly string[] ValidTypes = { ".pdf", ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" };
    }
    public enum ServiceName
    {
        Parser
    }
}
