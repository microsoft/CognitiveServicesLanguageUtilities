

using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
