using CliTool.Configs;
using CliTool.Configs.Models.Enums;
using CliTool.Services.Storage.StorageServices;

namespace CliTool.Services.Storage
{
    class StorageFactory : IStorageFactory
    {
        public IStorageService CreateDestinationStorageService(StorageType storageType, StorageConfigModel configs)
        {
            if (storageType.Equals(StorageType.Local))
            {
                return new LocalStorageService(configs.Local.DestinationDirectory);
            }
            if (storageType.Equals(StorageType.Blob))
            {
                return new BlobStorageService(configs.Blob.ConnectionString, configs.Blob.DestinationContainer);
            }
            return null;
        }

        public IStorageService CreateSourceStorageService(StorageType storageType, StorageConfigModel configs)
        {
            if (storageType.Equals(StorageType.Local))
            {
                return new LocalStorageService(configs.Local.SourceDirectory);
            }
            if (storageType.Equals(StorageType.Blob))
            {
                return new BlobStorageService(configs.Blob.ConnectionString, configs.Blob.SourceContainer);
            }
            return null;
        }
    }
}
