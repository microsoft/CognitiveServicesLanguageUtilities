using CliTool.Services.Configuration;
using CliTool.Services.Storage.StorageServices;

namespace CliTool.Services.Storage
{
    class StorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageConfigModel configs)
        {
            if (configs.ConnectionType == StorageType.Local) {
                return new LocalStorageService(configs.LocalDirectory);
            }
            if (configs.ConnectionType == StorageType.Blob)
            {
                return new BlobStorageService(configs.BlobStorageConnectionString, configs.BlobContainerName);
            }
            return null;
        }
    }
}
