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
        public Task<FileStream> ReadFile(string fileName);
        public void StoreFile(FileStream file);
    }
}
