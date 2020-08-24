using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CustomTextCliUtils.AppController.Models.ServiceModels.Prediction
{
    class CustomTextQueryResponse
    { }

    public partial class Welcome
    {
        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonProperty("lastActionDateTime")]
        public string LastActionDateTime { get; set; }
    }
}
}
