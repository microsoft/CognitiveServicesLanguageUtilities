using System;

using System.Globalization;
using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CliTool.Configs
{
    public class ConfigModel
    {
        [JsonProperty("storage")]
        public Storage Storage { get; set; }

        [JsonProperty("parser")]
        public Parser Parser { get; set; }
    }

    public class Parser
    {
        [JsonProperty("MSRead")]
        public MSReadConfigModel MsRead { get; set; }
    }

    public class Storage
    {
        [JsonProperty("source")]
        public StorageConfigModel Source { get; set; }

        [JsonProperty("destination")]
        public StorageConfigModel Destination { get; set; }
    }
}
