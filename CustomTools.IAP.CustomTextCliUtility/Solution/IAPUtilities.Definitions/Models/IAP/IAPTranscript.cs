// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.Enums.IAP;
using System.Collections.Generic;

namespace Microsoft.IAPUtilities.Definitions.Models.IAP
{
    public class IAPTranscript
    {
        public string Id { get; set; }
        public ChannelType Channel { get; set; }
        public List<ConversationUtterance> Utterances { get; set; }
    }
}
