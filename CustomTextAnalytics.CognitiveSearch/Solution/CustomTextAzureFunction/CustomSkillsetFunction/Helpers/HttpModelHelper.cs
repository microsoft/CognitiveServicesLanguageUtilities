// Copyright (c) Microsoft. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  

using Microsoft.AspNetCore.Http;
using Microsoft.CognitiveSearch.CustomSkillset.CustomText.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.CognitiveSearch.CustomSkillset.CustomText.Helpers
{
    public static class HttpModelHelper
    {
        public static IEnumerable<RequestRecord> GetRequestRecords(HttpRequest httpRequest)
        {
            var requestBodyJson = new StreamReader(httpRequest.Body).ReadToEnd();
            var docs = JsonConvert.DeserializeObject<CustomSkillsetRequest>(requestBodyJson);
            return docs.Values;
        }
    }
}