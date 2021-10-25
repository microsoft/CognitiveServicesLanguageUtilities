// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Document
{
    public class DocumentElement
    {
        public int? PageNumber { get; set; }
        public ElementType Type { get; set; }
        public string Text { get; set; }
    }
}
