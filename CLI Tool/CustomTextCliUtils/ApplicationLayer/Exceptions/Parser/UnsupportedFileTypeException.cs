using CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions;
using CustomTextCliUtils.Configs.Consts;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Parser
{
    class UnsupportedFileTypeException : CliException
    {
        public UnsupportedFileTypeException(string fileName, string fileType)
            :base(CliExceptionCode.UnsupportedFileType, ConstructMessage(fileName, fileType))
        { }

        public static string ConstructMessage(string fileName, string fileType)
        {
            return $"Unsupported file type {fileType} for file {fileName}\nSupported types are {Constants.ValidTypes.ToString()}";
        }
        
    }
}
