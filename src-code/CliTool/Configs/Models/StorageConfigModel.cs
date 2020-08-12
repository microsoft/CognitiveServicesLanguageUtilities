using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace CliTool.Services.Configuration
{
    class StorageConfigModel
    {
        public StorageType StorageType;
        public string Directory;
        public string ConnectionString;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    enum StorageType
    {
        Local,
        Blob
    }

}
