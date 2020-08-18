using CliTool.Configs.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.AppController.Factories.Storage
{
    class StorageFactoryFactory : IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage)
        {
            switch (targetStorage)
            {
                case TargetStorage.Source:
                        return new SourceStorageFactory();
                case TargetStorage.Destination:
                    return new DestinationStorageFactory();
                default:
                    return null; //TODO: throw exception
            }

        }
    }
}
