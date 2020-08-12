using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.Services.Storage.StorageServices
{
    class BlobStorageService : IStorageService
    {
        private static BlobContainerClient _blobContainerClient;
        public BlobStorageService(string connectionString, string containerName) {
            BlobServiceClient BlobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = BlobServiceClient.GetBlobContainerClient(containerName);
        }
        public string[] ListFiles()
        {
            var blobs = _blobContainerClient.GetBlobs();
            return blobs.Select(f => f.Name).ToArray();
        }

        public async Task<FileStream> ReadFile(string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content as FileStream;
        }

        public async void StoreFile(FileStream file)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(file.Name);
            await blobClient.UploadAsync(file, true);
            file.Close();
        }
    }
}
