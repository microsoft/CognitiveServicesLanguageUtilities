using FuzzyMatching.Definitions.Services;
using ProtoBuf;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _targetDirectory;

        public LocalStorageService(string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                throw new DirectoryNotFoundException();
            }
            _targetDirectory = targetDirectory;
        }

        public async Task StoreBinaryObjectAsync<T>(T data, string fileName, string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(_targetDirectory, relativePath, fileName);
                var file = File.Create(fullPath);
                Serializer.Serialize(file, data);
                file.Close();
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
                var fullPath = Path.Combine(_targetDirectory, relativePath, fileName);
                var file = File.OpenRead(fullPath);
                var result =  Serializer.Deserialize<T>(file);
                file.Close();
                return  await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new FileNotFoundException(e.Message);
            }
        }

        public async Task<string[]> ListPreprocessedDatasetsAsync(string Location)
        {
            string folderPath = Path.Combine(_targetDirectory, Location);
            return  Directory.GetFiles(folderPath).Select(i => Path.GetFileName(i)).ToArray();
        }
    }
}
