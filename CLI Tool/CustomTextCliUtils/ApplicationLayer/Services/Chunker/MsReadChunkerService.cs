
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using CustomTextCliUtils.Configs.Consts;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTextCliUtils.ApplicationLayer.Services.Chunker
{
    public class MsReadChunkerService : IChunkerService
    {
        public List<ChunkInfo> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit)
        {
            MsReadParseResult msReadparsingResult = parseResult as MsReadParseResult;
            switch (chunkMethod)
            {
                case ChunkMethod.NoChunking:
                    return ExtractText(msReadparsingResult);
                case ChunkMethod.Char:
                    return ChunkCharacters(msReadparsingResult, charLimit);
                case ChunkMethod.Page:
                    return ChunkPages(msReadparsingResult);
            }
            return null;
        }

        private List<ChunkInfo> ExtractText(MsReadParseResult parsingResult)
        {
            StringBuilder finalText = new StringBuilder();
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                foreach (Line l in rr.Lines)
                {
                    finalText.Append($"{l.Text} ");
                }
            }
            var text = finalText.ToString().Trim();
            return new List<ChunkInfo> {
                new ChunkInfo(text, 1, parsingResult.RecognitionResults.Count())
            };
        }

        private List<ChunkInfo> ChunkPages(MsReadParseResult parsingResult)
        {
            var pages = new List<ChunkInfo>();
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => l.BoundingBox[2] - l.BoundingBox[0]).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * 0.95)] * Constants.PercentageOfMaxLineLength;
            var paragraph = new StringBuilder();
            StringBuilder pageText = new StringBuilder();
            var currentPage = 0;
            var totalPageCount = parsingResult.RecognitionResults.Count;
            var pageStart = 1;
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults) // loop over each page to create list of pages
            {
                foreach (Line l in rr.Lines)
                {
                    paragraph.Append($"{l.Text} ");
                    if (IsLineEndOfParagraph(l, maxLineLength)) // end of paragraph
                    {
                        if (pageText.Length + paragraph.Length > Constants.CustomTextPredictionMaxCharLimit)
                        {
                            var text = pageText.ToString().Trim();
                            var chunkInfo = new ChunkInfo(text, pageStart, (int)rr.Page);
                            pages.Add(chunkInfo);
                            pageText.Clear();
                            pageStart = (int)rr.Page;
                        }
                        pageText.Append(paragraph.ToString());
                        paragraph.Clear();
                    }
                }
                if (paragraph.Length > 0 && ++currentPage == totalPageCount)
                {
                    pageText.Append(paragraph.ToString());
                }
                if (pageText.Length > 0)
                {
                    var text = pageText.ToString().Trim();
                    var chunkInfo = new ChunkInfo(text, pageStart, (int)rr.Page);
                    pages.Add(chunkInfo);
                    pageText.Clear();
                    pageStart = paragraph.Length > 0 ? (int)rr.Page : ((int)rr.Page + 1);
                } 
            }
            return pages;
        }

        // TODO: paragraph bigger than char limit
        private List<ChunkInfo> ChunkCharacters(MsReadParseResult parsingResult, int charLimit)
        {
            StringBuilder paragraph = new StringBuilder();
            StringBuilder chunk = new StringBuilder();
            List<ChunkInfo> chunks = new List<ChunkInfo>();
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => l.BoundingBox[2] - l.BoundingBox[0]).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * 0.95)] * Constants.PercentageOfMaxLineLength;
            var pageStart = 1;
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                foreach (Line l in rr.Lines)
                {
                    paragraph.Append($"{l.Text} ");
                    if (IsLineEndOfParagraph(l, maxLineLength)) // end of paragraph
                    {
                        if (chunk.Length + paragraph.Length > charLimit)
                        {
                            var text = chunk.ToString().Trim();
                            var chunkInfo = new ChunkInfo(text, pageStart, (int) rr.Page);
                            chunks.Add(chunkInfo);
                            chunk.Clear();
                            pageStart = (int)rr.Page;
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
                    var text = chunk.ToString().Trim();
                    var chunkInfo = new ChunkInfo(text, pageStart, parsingResult.RecognitionResults.Count);
                    chunks.Add(chunkInfo);
                }
                else
                {
                    var chunkInfo = new ChunkInfo(chunk.ToString(), pageStart, parsingResult.RecognitionResults.Count);
                    var paragraphInfo = new ChunkInfo(paragraph.ToString(), pageStart, parsingResult.RecognitionResults.Count);
                    chunks.Add(chunkInfo);
                    chunks.Add(paragraphInfo);
                }
            }
            return chunks;
        }

        private bool IsLineEndOfParagraph(Line line, double maxLineLength)
        {
            return line.BoundingBox[2] - line.BoundingBox[0] < maxLineLength;
        }
    }
}
