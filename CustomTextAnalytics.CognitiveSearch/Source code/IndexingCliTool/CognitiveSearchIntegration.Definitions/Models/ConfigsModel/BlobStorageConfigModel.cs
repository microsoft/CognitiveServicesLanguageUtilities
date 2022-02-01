﻿using Newtonsoft.Json;

namespace CognitiveSearchIntegration.Runners.Models.ConfigsModel
{
    public partial class BlobStorageConfigModel
    {
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty("containerName")]
        public string ContainerName { get; set; }
    }
}