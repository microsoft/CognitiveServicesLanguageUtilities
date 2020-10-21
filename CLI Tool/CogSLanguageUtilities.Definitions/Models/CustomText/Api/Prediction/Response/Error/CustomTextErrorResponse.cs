// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Error
{
    public class CustomTextErrorResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
