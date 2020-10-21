// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage
{
    public class StorageConfigModel
    {
        [JsonProperty("blob")]
        public BlobStorageConfigModel Blob { get; set; }

        [JsonProperty("local")]
        public LocalStorageConfigModel Local { get; set; }

        public StorageConfigModel()
        {
            Blob = new BlobStorageConfigModel();
            Local = new LocalStorageConfigModel();
        }
    }
}
