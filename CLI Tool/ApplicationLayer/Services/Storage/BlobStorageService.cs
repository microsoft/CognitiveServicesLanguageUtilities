using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomTextCliUtils.ApplicationLayer.Services.Storage
{
    class BlobStorageService : IStorageService
    {
        private BlobContainerClient _blobContainerClient;

        public BlobStorageService(string connectionString, string containerName) {
            try
            {
                 BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                if (!_blobContainerClient.Exists())
                {
                    throw new BlobContainerNotFoundException(containerName);
                }
            }
            catch (Exception e) when (e is RequestFailedException || e is FormatException || e is AggregateException)
            {
                throw new InvalidBlobStorageConnectionStringException(connectionString);
            }
        }

        public string[] ListFiles()
        {
            var blobs = _blobContainerClient.GetBlobs();
            return blobs.Select(f => f.Name).ToArray();
        }

        public Task<Stream> ReadFile(string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = blobClient.Download();
            var tcs = new TaskCompletionSource<Stream>();
            tcs.SetResult(download.Content);
            return tcs.Task;
        }

        public string ReadFileAsString(string fileName)
        {
            throw new NotImplementedException();
        }

        public void StoreData(string data, string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(data);
                    sw.Flush();
                    stream.Position = 0;
                    blobClient.Upload(stream, overwrite: true);
                }
            }
        }
    }
}
