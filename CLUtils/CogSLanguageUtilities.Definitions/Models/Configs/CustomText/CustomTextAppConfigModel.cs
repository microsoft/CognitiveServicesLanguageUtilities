// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText
{
    public class CustomTextAppConfigModel
    {
        [JsonProperty(ConfigKeys.CustomTextAzureResourceKey)]
        public string AzureResourceKey { get; set; }

        [JsonProperty(ConfigKeys.CustomTextAzureResourceEndpoint)]
        public string AzureResourceEndpoint { get; set; }

        [JsonProperty(ConfigKeys.CustomTextAppId)]
        public string AppId { get; set; }
    }
}
