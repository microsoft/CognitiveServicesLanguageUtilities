using CustomTextCliUtils.Configs.Models;
using CustomTextCliUtils.Configs.Models.Enums;
using CustomTextCliUtils.AppController.Services.Storage;

namespace CustomTextCliUtils.AppController.Factories.Storage
{
    internal class SourceStorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType storageType, StorageConfigModel storageConfigModel)
        {
            switch (storageType)
            {
                case StorageType.Blob:
                    return new BlobStorageService(storageConfigModel.Blob.ConnectionString, storageConfigModel.Blob.SourceContainer);
                case StorageType.Local:
                    return new LocalStorageService(storageConfigModel.Local.SourceDirectory);
                default:
                    return null;
            }
        }
    }
}