

using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage
{
    public interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
