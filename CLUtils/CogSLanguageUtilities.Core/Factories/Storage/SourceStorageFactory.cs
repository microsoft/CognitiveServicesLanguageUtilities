// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Services.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System;

namespace Microsoft.CogSLanguageUtilities.Core.Factories.Storage
{
    public class SourceStorageFactory : IStorageFactory
    {
        public IStorageService CreateStorageService(StorageType storageType, StorageConfigModel storageConfigModel)
        {
            switch (storageType)
            {
                case StorageType.Blob:
                    return new BlobStorageService(storageConfigModel.Blob.ConnectionString, storageConfigModel.Blob.SourceContainer);
                case StorageType.Local:
                    return new LocalStorageService(storageConfigModel.Local.SourceDirectory);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}