using CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Storage
{
    public class BlobContainerNotFoundException : CliException
    {
        public BlobContainerNotFoundException(string containerName)
            :base(CliExceptionCode.BlobContainerNotFound, ConstructMessage(containerName))
        { }

        public static string ConstructMessage(string containerName)
        {
            return "Blob Container Not Found: " + containerName;
        }
    }
}
