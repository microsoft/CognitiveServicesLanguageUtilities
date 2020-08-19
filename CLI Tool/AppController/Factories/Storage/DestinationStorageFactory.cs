using CustomTextCliUtils.AppController.Services.Storage;
using CustomTextCliUtils.Configs.Models;
using CustomTextCliUtils.Configs.Models.Enums;

namespace CustomTextCliUtils.AppController.Factories.Storage
{
    internal class DestinationStorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType storageType, StorageConfigModel storageConfigModel)
        {
            switch (storageType)
            {
                case StorageType.Blob:
                    return new BlobStorageService(storageConfigModel.Blob.ConnectionString, storageConfigModel.Blob.DestinationContainer);
                case StorageType.Local:
                    return new LocalStorageService(storageConfigModel.Local.DestinationDirectory);
                default:
                    return null;
            }
        }
    }
}