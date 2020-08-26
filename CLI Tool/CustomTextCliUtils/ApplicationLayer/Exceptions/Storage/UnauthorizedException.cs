using CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Storage
{
    public class UnauthorizedException : CliException
    {
        public UnauthorizedException(string accessType, string filePath)
            : base(CliExceptionCode.Unauthorized, ConstructMessage(accessType, filePath))
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
