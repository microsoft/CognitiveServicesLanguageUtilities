using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            return Directory.GetFiles(_targetDirectory);
        }

        public Task<FileStream> ReadFile(string fileName)
        {
            string fileDir = Path.Combine(_targetDirectory, Path.GetFileName(fileName));
            using (FileStream fs = File.OpenRead(fileDir)) {
                var tcs = new TaskCompletionSource<FileStream>();
                tcs.SetResult(fs);
                return tcs.Task;
            }
        }

        public void StoreFile(FileStream file)
        {
            string fileDir = Path.Combine(_targetDirectory, Path.GetFileName(file.Name));
            using (var fileStream = new FileStream(fileDir, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
        }
    }
}
