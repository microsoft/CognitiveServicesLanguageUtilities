using CliTool.Configs;
using CliTool.Configs.Models.Enums;
using CliTool.Services.Storage;


namespace CliTool.AppController.Factories.Storage
{
    interface IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType targetStorage, StorageConfigModel storageConfigModel);
    }
}
