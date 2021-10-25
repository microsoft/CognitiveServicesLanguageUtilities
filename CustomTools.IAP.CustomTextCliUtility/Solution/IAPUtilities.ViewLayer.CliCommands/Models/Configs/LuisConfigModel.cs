// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Newtonsoft.Json;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Models.Configs
{
    public class LuisConfigModel
    {
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }
    }
}
