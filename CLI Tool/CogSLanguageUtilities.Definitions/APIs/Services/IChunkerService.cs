using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IChunkerService
    {
        public List<ChunkInfo> Chunk(ParsedDocument parseResult, ChunkMethod chunkMethod, int charLimit);
    }
}
