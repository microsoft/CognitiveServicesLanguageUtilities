

using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
