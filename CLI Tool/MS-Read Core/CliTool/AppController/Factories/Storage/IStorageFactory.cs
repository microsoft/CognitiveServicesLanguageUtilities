using CustomTextCliUtils.AppController.Services.Storage;
using CustomTextCliUtils.Configs.Models;
using CustomTextCliUtils.Configs.Models.Enums;


namespace CustomTextCliUtils.AppController.Factories.Storage
{
    interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}
