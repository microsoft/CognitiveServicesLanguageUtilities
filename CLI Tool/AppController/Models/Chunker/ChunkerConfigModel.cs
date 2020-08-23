using CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTextCliUtils.AppController.Models.Chunker
{
    public class ChunkerConfigModel
    {
        [JsonProperty(ConfigKeys.ChunkerCharLimit)]
        public int CharLimit { get; set; }
    }
}
