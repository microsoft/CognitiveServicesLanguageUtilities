using CustomTextCliUtils.AppController.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTextCliUtils.AppController.Services.Chunker
{
    interface IChunkerService
    {
        public List<string> Chunk(ParseResult parseResult, ChunkMethod chunkMethod);
    }
}
