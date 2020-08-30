using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public class StorageFactoryFactory : IStorageFactoryFactory
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
