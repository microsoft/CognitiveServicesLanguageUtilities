using CliTool.Services.Configuration;
using CliTool.Services.Storage.StorageServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Services.Storage
{
    class StorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageConfigModel configs)
        {
            if (configs.StorageType == StorageType.Local) {
                return new LocalStorageService(configs.Directory);
            }
            if (configs.StorageType == StorageType.Blob)
            {
                return new BlobStorageService(configs.ConnectionString, configs.Directory);
            }
            return null;
        }
    }
}
