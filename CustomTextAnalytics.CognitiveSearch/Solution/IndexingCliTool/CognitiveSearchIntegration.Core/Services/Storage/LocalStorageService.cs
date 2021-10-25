using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        public async Task<string> ReadAsStringFromAbsolutePathAsync(string filePath)
        {
            if (await FileExistsAbsolutePath(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            else
            {
                throw new Definitions.Exceptions.Storage.FileNotFoundException(filePath);
            }
        }

        private async Task<bool> FileExistsAbsolutePath(string filePath)
        {
            return await Task.Run(() =>
            {
                return File.Exists(filePath);
            });
        }
    }
}
