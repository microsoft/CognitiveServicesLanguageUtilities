// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Newtonsoft.Json;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Models.Configs
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public StorageConfigModel Storage { get; set; }

        [JsonProperty("luis")]
        public LuisConfigModel Luis { get; set; }

        [JsonProperty("textAnalytics")]
        public TextAnalyticsConfigModel TextAnalytics { get; set; }
    }
}
