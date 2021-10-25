// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage
{
    public class LocalStorageConfigModel
    {
        [JsonProperty(ConfigKeys.LocalStorageSourceDir)]
        public string SourceDirectory { get; set; }

        [JsonProperty(ConfigKeys.LocalStorageDestinationDir)]
        public string DestinationDirectory { get; set; }
    }
}