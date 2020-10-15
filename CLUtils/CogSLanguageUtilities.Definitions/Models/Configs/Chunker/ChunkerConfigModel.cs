// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker
{
    public class ChunkerConfigModel
    {
        [JsonProperty(ConfigKeys.ChunkerCharLimit)]
        public int CharLimit { get; set; }
        [JsonProperty(ConfigKeys.ChunkerSectionLevel)]
        public ElementType ChunkSectionLevel { get; set; }
    }
}
