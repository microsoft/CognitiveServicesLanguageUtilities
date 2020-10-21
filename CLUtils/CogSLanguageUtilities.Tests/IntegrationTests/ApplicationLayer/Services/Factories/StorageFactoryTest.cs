// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Core.Services.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.Tests.Configs;
using System;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.IntegrationTests.Services.Factories
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
