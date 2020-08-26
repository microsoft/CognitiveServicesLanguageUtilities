using CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Storage
{
    public class InvalidBlobStorageConnectionStringException : CliException
    {
        public InvalidBlobStorageConnectionStringException(string connectionString)
            : base(CliExceptionCode.InvalidBlobStorageConnectionString, ConstructMessage(connectionString))
        { }

        public static string ConstructMessage(string connectionString)
        {
            return "Invalid Blob Storage Connection String: " + connectionString;
        }
    }
}
