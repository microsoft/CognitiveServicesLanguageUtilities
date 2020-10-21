// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response
{
    public class Example
    {
        [JsonProperty("document")]
        public Document Document { get; set; }

        [JsonProperty("classificationLabels")]
        public ClassificationLabel[] ClassificationLabels { get; set; }

        [JsonProperty("miniDocs")]
        public List<MiniDoc> MiniDocs { get; set; }
    }
}
