// Copyright (c) Microsoft. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  

using System.Collections.Generic;

namespace Microsoft.CognitiveSearch.CustomSkillset.CustomText.ViewModels
{
    public class ResponseRecord
    {
        public string RecordId { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public List<ErrorWarning> Errors { get; set; } = new List<ErrorWarning>();
        public List<ErrorWarning> Warnings { get; set; } = new List<ErrorWarning>();
    }
}