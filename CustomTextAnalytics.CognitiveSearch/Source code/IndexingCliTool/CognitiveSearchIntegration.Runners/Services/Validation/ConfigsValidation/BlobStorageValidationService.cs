using Azure.Storage.Blobs;
using System;
using System.Threading.Tasks;

namespace CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation
{
    /// <summary>
    /// we need to validate both connection string and container name are correct
    /// we also need to make sure we have access rights to blobs in the container
    /// listing blobs in a container will both validate both connection string and container name
    /// </summary>
    public class BlobStorageValidationService
    {
        public async Task ValidateBlobConfigsAsync(string connectionString, string containerName)
        {
            try
            {
                await ListBlobsInContainer(connectionString, containerName);
            }
            catch (Exception)
            {
                throw new Exception("Blob Configs Are Invalid!");
            }
        }

        private async Task ListBlobsInContainer(string connectionString, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var resultSegment = blobContainerClient.GetBlobsAsync().AsPages();
            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                // do nothing
            }
        }
    }
}
