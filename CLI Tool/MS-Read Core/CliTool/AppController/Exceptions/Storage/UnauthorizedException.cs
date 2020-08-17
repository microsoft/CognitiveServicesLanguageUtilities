namespace CliTool.Exceptions.Storage
{
    class UnauthorizedException : CliException
    {
        public UnauthorizedException(string accessType, string filePath)
        {
            CustomMessage = "Unauthorized " + accessType + " for file " + filePath;
        }
    }

    public enum AccessType
    {
        Read, Write
    }
}
