using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services
{
    public interface IStorageService
    {
        public Task<string> ReadAsStringFromAbsolutePathAsync(string filePath);
    }
}