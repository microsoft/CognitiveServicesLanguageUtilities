using CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Factories
{
    public class StorageFactoryFactoryTest
    {
        public static TheoryData StorageFactoryCreationTestData()
        {
            return new TheoryData<TargetStorage, Type>
            {
                {
                    TargetStorage.Destination,
                    typeof(DestinationStorageFactory)
                },
                {
                    TargetStorage.Source,
                    typeof(SourceStorageFactory)
                }
            };
        }

        [Theory]
        [MemberData(nameof(StorageFactoryCreationTestData))]
        public void StorageFactoryCreationTest(TargetStorage targetStorage, Type factoryType)
        {
            StorageFactoryFactory factoryFactory = new StorageFactoryFactory();
            IStorageFactory storageFactory = factoryFactory.CreateStorageFactory(targetStorage);
            Assert.True(storageFactory.GetType().Equals(factoryType));
        }
    }
}
