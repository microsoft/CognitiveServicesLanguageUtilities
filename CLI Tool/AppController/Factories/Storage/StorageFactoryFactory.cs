using CustomTextCliUtils.AppController.Models.Enums;

namespace CustomTextCliUtils.AppController.Factories.Storage
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
