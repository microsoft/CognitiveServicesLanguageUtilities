using CustomTextCliUtils.AppController.Models.Enums;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTextCliUtils.AppController.Services.Chunker
{
    class MsReadChunkerService : IChunkerService
    {
        public List<string> Chunk(ParseResult parseResult, ChunkMethod chunkMethod)
        {
            MsReadParseResult msReadChunkingInput = parseResult as MsReadParseResult;
            switch (chunkMethod)
            {
                case ChunkMethod.NoChunking:
                    return ExtractText(msReadChunkingInput);
                case ChunkMethod.Char:
                    return ChunkCharacters(msReadChunkingInput);
                case ChunkMethod.Page:
                    return ChunkPages(msReadChunkingInput);
            }
            return null;
        }

        private List<string> ExtractText(MsReadParseResult chunkingInput)
        {
            StringBuilder finalText = new StringBuilder();
            foreach (TextRecognitionResult rr in chunkingInput.RecognitionResults)
            {
                foreach (Line l in rr.Lines)
                {
                    finalText.Append($"{l.Text} ");
                }
            }
            return new List<string>() { finalText.ToString() };
        }

        private List<string> ChunkPages(MsReadParseResult chunkingInput)
        {
            List<string> pages = new List<string>();
            foreach (TextRecognitionResult rr in chunkingInput.RecognitionResults)
            {
                StringBuilder pageText = new StringBuilder();
                foreach (Line l in rr.Lines)
                {
                    pageText.Append($"{l.Text} ");
                }
                pages.Add(pageText.ToString());
            }
            return pages;
        }

        private List<string> ChunkCharacters(MsReadParseResult chunkingInput)
        {
            throw new NotImplementedException();
        }
    }
}
