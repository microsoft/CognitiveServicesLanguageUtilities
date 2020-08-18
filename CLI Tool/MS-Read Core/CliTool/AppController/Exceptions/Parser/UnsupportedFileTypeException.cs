using CliTool.Configs;
using CliTool.Configs.Consts;

namespace CliTool.Exceptions.Parser
{
    class UnsupportedFileTypeException : CliException
    {
        public UnsupportedFileTypeException(string fileName, string fileType)
        {
            CustomMessage = "Unsupported file type " + fileType + "for file " + fileName 
                + "\nSupported types are " + Constants.ValidTypes.ToString();
        }
    }
}
