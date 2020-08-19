using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace CustomTextCliUtils.AppController.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StorageType
    {
        Local,
        Blob,
        Null
    }
}




