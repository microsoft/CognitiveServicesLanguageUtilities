using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FuzzyMatching.Definitions.Services;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Storage
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
                    throw new FileLoadException();
                    // throw new BlobContainerNotFoundException(containerName);
                }
            }
            catch (Exception e) when (e is RequestFailedException || e is FormatException || e is AggregateException)
            {
                throw new Exception();
            }
        }

      



        public async Task StoreBinaryObjectAsync<T>(T data, string fileName, string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(relativePath, fileName);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fullPath);
                using (MemoryStream stream = new MemoryStream())
                {

                    Serializer.Serialize(stream, data);
                    stream.Position = 0;
                    await blobClient.UploadAsync(stream, overwrite: true);
                    
                }
            }
            catch (Exception e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task<T> LoadBinaryObjectAsync<T>(string fileName, string relativePath)
        {
            try
            {
                var fullPath = Path.Combine( relativePath, fileName);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fullPath);
                BlobDownloadInfo download = await blobClient.DownloadAsync();
                return Serializer.Deserialize<T>( download.Content);
            }
            catch (Exception e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task<string[]> ListPreprocessedDatasetsAsync(string Location)
        {
            var blobs = _blobContainerClient.GetBlobsAsync();
            List<string> blobNames = new List<string>();
            await foreach (BlobItem blob in blobs)
            {
                blobNames.Add(blob.Name);
            }
            return blobNames.ToArray();
        }


    }
}