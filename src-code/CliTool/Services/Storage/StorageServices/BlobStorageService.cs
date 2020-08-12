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

        public async Task<Stream> ReadFile(string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }

        public async void StoreData(string data, string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(data);
                    sw.Flush();
                    stream.Position = 0;
                    await blobClient.UploadAsync(stream, overwrite: true).ConfigureAwait(false);
                }
            }
        }
    }
}
