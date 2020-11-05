// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IChunkerService
    {
        public List<ChunkInfo> Chunk(DocumentTree documentTree, ChunkMethod chunkMethod, int charLimit, ElementType chunkLevel);
    }
}
