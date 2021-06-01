using System.Threading.Tasks;

namespace FuzzyMatching.Definitions.Services
{
    public interface IStorageService
    {
        public Task StoreBinaryObjectAsync<T>(T data, string fileName, string relativePath);
        public Task<T> LoadBinaryObjectAsync<T>(string fileName, string relativePath);
        public Task<string[]> ListPreprocessedDatasetsAsync(string relativePath);
    }
}
