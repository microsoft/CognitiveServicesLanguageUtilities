using CliTool.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Services.Storage
{
    interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageConfigModel configs);
    }
}
