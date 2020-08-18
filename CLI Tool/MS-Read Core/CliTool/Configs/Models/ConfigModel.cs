using System;

using CliTool.Services.Configuration.Models;
using Newtonsoft.Json;

namespace CliTool.Configs
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public StorageConfigModel Storage { get; set; }

        [JsonProperty("parser")]
        public ParserConfigModel Parser { get; set; }
    }

    public class ParserConfigModel
    {
        [JsonProperty("MSRead")]
        public MSReadConfigModel MsRead { get; set; }
    }

    public class StorageConfigModel
    {
        [JsonProperty("blob")]
        public BlobStorageConfigModel Blob { get; set; }

        [JsonProperty("local")]
        public LocalStorageConfigModel Local { get; set; }
    }
}
