using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.Services.Storage
{
    interface IStorageService
    {
        public string[] ListFiles();
        public Task<Stream> ReadFile(string fileName);
        public void StoreData(string data, string fileName);
        public string ReadFileAsString(string fileName);
    }
}
