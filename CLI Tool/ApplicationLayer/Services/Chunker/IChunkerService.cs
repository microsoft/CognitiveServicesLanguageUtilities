using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using System.Collections.Generic;

namespace CustomTextCliUtils.ApplicationLayer.Services.Chunker
{
    interface IChunkerService
    {
        public List<string> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit);
    }
}
