using System.IO;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.Services.Storage
{
    interface IStorageService
    {
        public string[] ListFiles();
        public Task<Stream> ReadFile(string fileName);
        public void StoreData(string data, string fileName);
        public string ReadFileAsString(string fileName);
    }
}
