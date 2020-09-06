using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage
{
    public class BlobStorageService : IStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(string connectionString, string containerName)
        {
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

        public async Task<string[]> ListFilesAsync()
        {
            var blobs = _blobContainerClient.GetBlobsAsync();
            List<string> blobNames = new List<string>();
            await foreach (BlobItem blob in blobs)
            {
                blobNames.Add(blob.Name);
            }
            return blobNames.ToArray();
        }

        public async Task<Stream> ReadFileAsync(string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }

        public async Task<string> ReadFileAsStringAsync(string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            using (StreamReader sr = new StreamReader(download.Content))
            {
                return sr.ReadToEnd();
            }
        }

        public async Task StoreDataAsync(string data, string fileName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(data);
                    sw.Flush();
                    stream.Position = 0;
                    await blobClient.UploadAsync(stream, overwrite: true);
                }
            }
        }
    }
}
