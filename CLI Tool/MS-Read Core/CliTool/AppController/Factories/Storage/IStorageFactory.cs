using CliTool.Configs;
using CliTool.Configs.Models.Enums;
using CliTool.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Services.Storage
{
    interface IStorageFactory
    {
        public IStorageService CreateSourceStorageService(StorageType storageType, StorageConfigModel configs);
        public IStorageService CreateDestinationStorageService(StorageType storageType, StorageConfigModel configs);
    }
}
