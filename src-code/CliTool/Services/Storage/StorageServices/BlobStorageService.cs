using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CliTool.Services.Storage.StorageServices
{
    class BlobStorageService : IStorageService
    {
        private BlobContainerClient _blobContainerClient;
        public BlobStorageService(string connectionString, string containerName) {
            BlobServiceClient BlobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = BlobServiceClient.GetBlobContainerClient(containerName);
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
