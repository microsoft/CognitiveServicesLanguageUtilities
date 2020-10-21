// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage
{
    public interface IStorageFactoryFactory
    {
        public IStorageFactory CreateStorageFactory(TargetStorage targetStorage);
    }
}
