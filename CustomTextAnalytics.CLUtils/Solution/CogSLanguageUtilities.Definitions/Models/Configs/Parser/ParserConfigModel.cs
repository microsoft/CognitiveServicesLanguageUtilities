// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser
{
    public class ParserConfigModel
    {
        [JsonProperty("msread")]
        public MSReadConfigModel MsRead { get; set; }

        public ParserConfigModel()
        {
            MsRead = new MSReadConfigModel();
        }
    }
}
