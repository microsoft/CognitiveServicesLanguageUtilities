namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Parser
{
    class UnsupportedFileTypeException : CliException
    {
        public UnsupportedFileTypeException(string fileName, string fileType, string[] validTypes)
            :base(ConstructMessage(fileName, fileType, validTypes))
        { }

        public static string ConstructMessage(string fileName, string fileType, string[] validTypes)
        {
            return $"Unsupported file type {fileType} for file {fileName} (Supported types are {string.Join(", ", validTypes)})";
        }
        
    }
}
