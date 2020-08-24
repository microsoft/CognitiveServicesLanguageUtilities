using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;


namespace CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}
