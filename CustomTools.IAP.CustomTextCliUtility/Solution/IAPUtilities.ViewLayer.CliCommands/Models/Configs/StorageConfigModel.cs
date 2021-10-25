// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Newtonsoft.Json;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Models.Configs
{
    public class StorageConfigModel
    {
        [JsonProperty("sourcePath")]
        public string SourcePath { get; set; }

        [JsonProperty("destinationPath")]
        public string DestinationPath { get; set; }
    }
}
