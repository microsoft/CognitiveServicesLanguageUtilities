using CliTool.Exceptions;
using CliTool.Exceptions.Storage;
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
            if (!Directory.Exists(_targetDirectory))
            {
                throw new FolderNotFoundException(targetDirectory);
            }
            _targetDirectory = targetDirectory;
        }

        public string[] ListFiles()
        {
            return Directory.GetFiles(_targetDirectory).Select(i => Path.GetFileName(i)).ToArray();
        }

        public Task<Stream> ReadFile(string fileName)
        {
            string fileDir = Path.Combine(_targetDirectory, fileName);
            var tcs = new TaskCompletionSource<Stream>();
            try
            {
                FileStream fs = File.OpenRead(fileDir);
                tcs.SetResult(fs as Stream);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedException(AccessType.Read.ToString(), Path.Combine(_targetDirectory, fileName));
            }
            return tcs.Task;
        }

        public void StoreData(string data, string fileName)
        {
            try 
            { 
                string fileDir = Path.Combine(_targetDirectory, Path.GetFileName(fileName));
                File.WriteAllText(fileDir, data);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedException(AccessType.Write.ToString(), fileName);
            }
        }
    }
}
