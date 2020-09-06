using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _targetDirectory;

        public LocalStorageService(string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                throw new FolderNotFoundException(targetDirectory);
            }
            _targetDirectory = targetDirectory;
        }

        public Task<string[]> ListFilesAsync()
        {
            return Task.FromResult(Directory.GetFiles(_targetDirectory).Select(i => Path.GetFileName(i)).ToArray());
        }

        public Task<Stream> ReadFileAsync(string fileName)
        {
            string filePath = Path.Combine(_targetDirectory, fileName);
            try
            {
                FileStream fs = File.OpenRead(filePath);
                return Task.FromResult(fs as Stream);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedFileAccessException(AccessType.Read.ToString(), Path.Combine(_targetDirectory, fileName));
            }
        }

        public async Task<string> ReadFileAsStringAsync(string fileName)
        {
            return await File.ReadAllTextAsync(Path.Combine(_targetDirectory, fileName));
        }

        public async Task StoreDataAsync(string data, string fileName)
        {
            try
            {
                string filePath = Path.Combine(_targetDirectory, Path.GetFileName(fileName));
                await File.WriteAllTextAsync(filePath, data);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedFileAccessException(AccessType.Write.ToString(), fileName);
            }
        }
    }
}
