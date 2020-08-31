using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Tests.Configs;
using System;
using Xunit;

namespace Microsoft.CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Factories
{
    public class StorageFactoryTest
    {
        public static TheoryData StorageServiceCreationTestData()
        {
            BlobStorageConfigModel blobStorageConfigModel = new BlobStorageConfigModel
            {
                ConnectionString = Secrets.StorageAccountConnectionString,
                SourceContainer = "container1",
                DestinationContainer = "container2"
            };
            LocalStorageConfigModel localStorageConfigModel = new LocalStorageConfigModel
            {
                SourceDirectory = "./",
                DestinationDirectory = "./"
            };

            return new TheoryData<StorageType, StorageConfigModel, Type>
            {
                {
                    StorageType.Blob,
                    new StorageConfigModel()
                    {
                        Local = localStorageConfigModel,
                        Blob = blobStorageConfigModel
                    },
                    typeof(BlobStorageService)
                },
                {
                    StorageType.Local,
                    new StorageConfigModel()
                    {
                        Local = localStorageConfigModel,
                        Blob = blobStorageConfigModel
                    },
                    typeof(LocalStorageService)
                }
            };
        }

        [Theory]
        [MemberData(nameof(StorageServiceCreationTestData))]
        public void DestinationStorageServiceCreationTest(StorageType storageType, StorageConfigModel configModel, Type storageServiceType)
        {
            IStorageFactory storageFactory = new DestinationStorageFactory();
            IStorageService storageService = storageFactory.CreateStorageService(storageType, configModel);
            Assert.Equal(storageServiceType, storageService.GetType());
        }

        [Theory]
        [MemberData(nameof(StorageServiceCreationTestData))]
        public void SourceStorageServiceCreationTest(StorageType storageType, StorageConfigModel configModel, Type storageServiceType)
        {
            IStorageFactory storageFactory = new SourceStorageFactory();
            IStorageService storageService = storageFactory.CreateStorageService(storageType, configModel);
            Assert.Equal(storageServiceType, storageService.GetType());
        }
    }
}
