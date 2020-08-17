using System;

namespace CliTool.Exceptions.Storage
{
    class BlobContainerNotFoundException : CliException
    {
        public BlobContainerNotFoundException(string containerName)
        {
            CustomMessage = "Blob Container Not Found: " + containerName;
        }
    }
}
