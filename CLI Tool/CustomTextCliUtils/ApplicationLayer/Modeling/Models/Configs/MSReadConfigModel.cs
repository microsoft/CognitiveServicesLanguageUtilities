﻿using CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class MSReadConfigModel
    {
        [JsonProperty(ConfigKeys.MSReadCognitiveServicesEndpoint)]
        public string CognitiveServiceEndPoint { get; set; }

        [JsonProperty(ConfigKeys.MSReadCognitiveServicesKey)]
        public string CongnitiveServiceKey { get; set; }
    }
}