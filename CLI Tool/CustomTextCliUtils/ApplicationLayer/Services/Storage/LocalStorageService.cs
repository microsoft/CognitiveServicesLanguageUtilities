using CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomTextCliUtils.ApplicationLayer.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        private string _targetDirectory;

        public LocalStorageService(string targetDirectory) {
            if (!Directory.Exists(targetDirectory))
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
            string filePath = Path.Combine(_targetDirectory, fileName);
            var tcs = new TaskCompletionSource<Stream>();
            try
            {
                FileStream fs = File.OpenRead(filePath);
                tcs.SetResult(fs as Stream);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedException(AccessType.Read.ToString(), Path.Combine(_targetDirectory, fileName));
            }
            return tcs.Task;
        }

        public string ReadFileAsString(string fileName)
        {
            return File.ReadAllText(Path.Combine(_targetDirectory, fileName));
        }

        public void StoreData(string data, string fileName)
        {
            try 
            { 
                string filePath = Path.Combine(_targetDirectory, Path.GetFileName(fileName));
                File.WriteAllText(filePath, data);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedException(AccessType.Write.ToString(), fileName);
            }
        }
    }
}
