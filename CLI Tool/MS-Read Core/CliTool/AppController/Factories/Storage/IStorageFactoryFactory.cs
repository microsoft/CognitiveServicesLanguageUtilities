using CliTool.Configs.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.AppController.Factories.Storage
{
    interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
