﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models
{
    public class CustomTextSchema
    {
        [JsonProperty("entityNames")]
        public List<string> EntityNames { get; set; }
    }
}
