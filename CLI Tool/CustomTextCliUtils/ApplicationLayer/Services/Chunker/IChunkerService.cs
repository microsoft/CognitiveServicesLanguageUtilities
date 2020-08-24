using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using System.Collections.Generic;

namespace CustomTextCliUtils.ApplicationLayer.Services.Chunker
{
    interface IChunkerService
    {
        public List<ChunkInfo> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit);
    }
}
