// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.IntegrationTests.Services.Factories
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
