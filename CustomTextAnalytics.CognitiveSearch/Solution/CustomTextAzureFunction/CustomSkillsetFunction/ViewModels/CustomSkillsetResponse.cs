// Copyright (c) Microsoft. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  

using System.Collections.Generic;

namespace Microsoft.CognitiveSearch.CustomSkillset.CustomText.ViewModels
{
    public class CustomSkillsetResponse
    {
        public List<ResponseRecord> Values { get; set; } = new List<ResponseRecord>();
    }
}