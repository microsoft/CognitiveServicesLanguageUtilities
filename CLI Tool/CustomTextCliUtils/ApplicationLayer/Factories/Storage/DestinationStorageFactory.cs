using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public class DestinationStorageFactory : IStorageFactory
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