using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CliTool.Services.Storage.StorageServices
{
    class LocalStorageService : IStorageService
    {
        private string _targetDirectory;
        public LocalStorageService(string targetDirectory) {
            _targetDirectory = targetDirectory;
            if (Directory.Exists(_targetDirectory) == false) { 
                // throw error
            }
        }

        public string[] ListFiles()
        {
            return Directory.GetFiles(_targetDirectory).Select(i => Path.GetFileName(i)).ToArray();
        }

        public Task<Stream> ReadFile(string fileName)
        {
            string fileDir = Path.Combine(_targetDirectory, Path.GetFileName(fileName));
            var tcs = new TaskCompletionSource<Stream>();
            try
            {
                FileStream fs = File.OpenRead(fileDir);
                tcs.SetResult(fs as Stream);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
            return tcs.Task;
        }

        public void StoreData(string data, string fileName)
        {
            string fileDir = Path.Combine(_targetDirectory, Path.GetFileName(fileName));
            File.WriteAllText(fileDir, data);
        }
    }
}
