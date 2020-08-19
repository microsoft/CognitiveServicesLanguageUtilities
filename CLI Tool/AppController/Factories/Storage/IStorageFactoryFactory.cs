using CustomTextCliUtils.AppController.Models.Enums;

namespace CustomTextCliUtils.AppController.Factories.Storage
{
    interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
