// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage
{
    public class BlobStorageConfigModel
    {
        [JsonProperty(ConfigKeys.BlobStorageConnectionstring)]
        public string ConnectionString { get; set; }

        [JsonProperty(ConfigKeys.BlobStorageSourceContainer)]
        public string SourceContainer { get; set; }

        [JsonProperty(ConfigKeys.BlobStorageDestinationContainer)]
        public string DestinationContainer { get; set; }
    }
}