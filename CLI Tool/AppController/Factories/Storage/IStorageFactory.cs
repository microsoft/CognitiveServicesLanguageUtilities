using CustomTextCliUtils.AppController.Services.Storage;
using CustomTextCliUtils.AppController.Models;
using CustomTextCliUtils.AppController.Models.Enums;


namespace CustomTextCliUtils.AppController.Factories.Storage
{
    interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}
