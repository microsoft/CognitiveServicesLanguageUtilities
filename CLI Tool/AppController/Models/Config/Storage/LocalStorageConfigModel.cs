using CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace CustomTextCliUtils.AppController.Models.Config.Storage
{
    public class LocalStorageConfigModel
    {
        [JsonProperty(ConfigKeys.LocalStorageSourceDir)]
        public string SourceDirectory { get; set; }

        [JsonProperty(ConfigKeys.LocalStorageDestinationDir)]
        public string DestinationDirectory { get; set; }
    }
}