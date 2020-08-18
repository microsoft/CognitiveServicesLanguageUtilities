using CliTool.Configs.Models.Enums;
using CliTool.Exceptions;
using CliTool.Exceptions.Storage;
using CliTool.Services.Logger;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CliTool.Services.Storage.StorageServices
{
    class LocalStorageService : IStorageService
    {
        private string _targetDirectory;
        ILoggerService _loggerService = new ConsoleLoggerService();

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
            _loggerService.LogOperation(OperationType.ReadingFile, filePath + " from disk");
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
                _loggerService.LogOperation(OperationType.StoringResult, filePath + " to disk");

                File.WriteAllText(filePath, data);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedException(AccessType.Write.ToString(), fileName);
            }
        }
    }
}
