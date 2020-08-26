﻿using CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;
using Xunit;

namespace CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Factories
{
    public class StorageFactoryTest
    {
        public static TheoryData StorageServiceCreationTestData()
        {
            BlobStorageConfigModel blobStorageConfigModel = new BlobStorageConfigModel
            {
                ConnectionString = "DefaultEndpointsProtocol=https;AccountName=nourdocuments;AccountKey=5UvtQ8CiXwDXg63QyEgtReW3E31KTXMvT5UfjnX1XgAW1DU390nKAlkCeBn7DUyDgaaQdm5TZt3iB7DfdUlD5A==;EndpointSuffix=core.windows.net",
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