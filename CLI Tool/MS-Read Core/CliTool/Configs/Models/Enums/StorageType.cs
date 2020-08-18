using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CliTool.Configs.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StorageType
    {
        Local,
        Blob,
        Null
    }
}




