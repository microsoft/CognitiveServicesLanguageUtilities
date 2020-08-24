using CustomTextCliUtils.AppController.Models.Enums;
using CustomTextCliUtils.AppController.Models.ServiceModels.Parser;
using CustomTextCliUtils.Configs.Consts;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTextCliUtils.AppController.Services.Chunker
{
    class MsReadChunkerService : IChunkerService
    {
        public List<string> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit)
        {
            MsReadParseResult msReadChunkingInput = parseResult as MsReadParseResult;
            switch (chunkMethod)
            {
                case ChunkMethod.NoChunking:
                    return ExtractText(msReadChunkingInput);
                case ChunkMethod.Char:
                    return ChunkCharacters(msReadChunkingInput, charLimit);
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

        private List<string> ChunkPages(MsReadParseResult parsingResult)
        {
            List<string> pages = new List<string>();
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => l.BoundingBox[2] - l.BoundingBox[0]).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * 0.95)] * Constants.PercentageOfMaxLineLength;
            var paragraph = new StringBuilder();
            var extendedParagraph = false;
            StringBuilder pageText = new StringBuilder();
            var i = 0;
            var count = parsingResult.RecognitionResults.Count;
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                foreach (Line l in rr.Lines)
                {
                    pageText.Append($"{l.Text} ");
                    if (extendedParagraph && l.BoundingBox[2] - l.BoundingBox[0] < maxLineLength)
                    {
                        pages.Add(pageText.ToString());
                        pageText.Clear();
                        extendedParagraph = false;
                    }
                }
                if (++i == count || rr.Lines.Last().BoundingBox[2] - rr.Lines.Last().BoundingBox[0] < maxLineLength)
                {
                    pages.Add(pageText.ToString());
                    pageText.Clear();
                }
                else
                {
                    extendedParagraph = true;
                }
            }
            return pages;
        }

        // TODO: paragraph bigger than char limit
        private List<string> ChunkCharacters(MsReadParseResult parsingResult, int charLimit)
        {
            StringBuilder paragraph = new StringBuilder();
            StringBuilder chunk = new StringBuilder();
            List<string> chunks = new List<string>();
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => l.BoundingBox[2] - l.BoundingBox[0]).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * 0.95)] * Constants.PercentageOfMaxLineLength;
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                foreach (Line l in rr.Lines)
                {
                    paragraph.Append($"{l.Text} ");
                    if (l.BoundingBox[2] - l.BoundingBox[0] < maxLineLength) // end of paragraph
                    {
                        if (chunk.Length + paragraph.Length > charLimit)
                        {
                            chunks.Add(chunk.ToString());
                            chunk.Clear();
                        }
                        chunk.Append(paragraph.ToString());
                        paragraph.Clear();
                    }
                }
            }
            if (paragraph.Length > 0)
            {
                if (chunk.Length + paragraph.Length <= charLimit)
                {
                    chunk.Append(paragraph.ToString());
                    chunks.Add(chunk.ToString());
                }
                else
                {
                    chunks.Add(chunk.ToString());
                    chunks.Add(paragraph.ToString());
                }
            }
            return chunks;
        }
    }
}
