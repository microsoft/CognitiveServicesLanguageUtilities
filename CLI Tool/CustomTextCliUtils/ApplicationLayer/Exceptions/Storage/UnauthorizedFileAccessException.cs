namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Storage
{
    public class UnauthorizedFileAccessException : CliException
    {
        public UnauthorizedFileAccessException(string accessType, string filePath)
            : base(ConstructMessage(accessType, filePath))
        { }

        public static string ConstructMessage(string accessType, string filePath)
        {
            return "Unauthorized " + accessType + " for file " + filePath;
        }
    }

    public enum AccessType
    {
        Read, Write
    }
}
