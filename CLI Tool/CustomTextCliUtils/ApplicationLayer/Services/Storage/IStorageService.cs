using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage
{
    public interface IStorageService
    {
        public string[] ListFiles();
        public Task<Stream> ReadFileAsync(string fileName);
        public Task StoreDataAsync(string data, string fileName);
        public Task<string> ReadFileAsStringAsync(string fileName);
    }
}
