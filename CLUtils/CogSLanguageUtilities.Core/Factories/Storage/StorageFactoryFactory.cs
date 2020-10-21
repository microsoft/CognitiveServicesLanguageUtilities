// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System;

namespace Microsoft.CogSLanguageUtilities.Core.Factories.Storage
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
                    throw new NotSupportedException($"The target storage {targetStorage} isn't supported.");
            }
        }
    }
}
