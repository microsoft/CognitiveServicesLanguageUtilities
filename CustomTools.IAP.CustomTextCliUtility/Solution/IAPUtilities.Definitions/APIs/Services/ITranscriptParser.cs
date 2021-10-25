// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.Models.IAP;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface ITranscriptParser
    {
        public Task<IAPTranscript> ParseTranscriptAsync(Stream file);
    }
}