using CliTool.Configs;
using CliTool.Configs.Consts;

namespace CliTool.Exceptions.Parser
{
    class UnsupportedFileTypeException : CliException
    {
        public UnsupportedFileTypeException(string fileName, string fileType)
            :base(ConstructMessage(fileName, fileType))
        { }

        public static string ConstructMessage(string fileName, string fileType)
        {
            return "Unsupported file type " + fileType + "for file " + fileName + "\nSupported types are " + Constants.ValidTypes.ToString();
        }
        
    }
}
