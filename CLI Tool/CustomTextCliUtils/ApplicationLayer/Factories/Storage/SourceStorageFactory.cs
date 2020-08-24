
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;

namespace CustomTextCliUtils.ApplicationLayer.Factories.Storage
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