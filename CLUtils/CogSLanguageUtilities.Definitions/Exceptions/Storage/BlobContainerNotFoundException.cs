namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Storage
{
    public class BlobContainerNotFoundException : CliException
    {
        public BlobContainerNotFoundException(string containerName)
            : base(ConstructMessage(containerName))
        { }

        public static string ConstructMessage(string containerName)
        {
            return "Blob Container Not Found: " + containerName;
        }
    }
}
