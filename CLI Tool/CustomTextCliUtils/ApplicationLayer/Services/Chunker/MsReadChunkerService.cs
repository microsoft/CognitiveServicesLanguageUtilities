using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker
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
                default:
                    throw new NotSupportedException();
            }
        }

        /*
         *  MsReadParseResult is a list of pages and each page contains a list of lines
            This function joins the lines in a single string 
         */
        private List<ChunkInfo> ExtractText(MsReadParseResult parsingResult)
        {
            StringBuilder finalText = new StringBuilder();
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                finalText.Append(string.Join(' ', rr.Lines.Select(l => l.Text)));
            }
            var text = finalText.ToString().Trim();
            return new List<ChunkInfo> 
            {
                new ChunkInfo(text, 1, parsingResult.RecognitionResults.Count())
            };
        }

        /*
         *  Data structure: 
         *      MsReadParseResult is a list of pages and each page contains a list of lines
         *  Intuition:
         *      To construct a page, we concatenate paragraphs in the page to the chunk
         *  Considerations:
         *      1- Chunk doesn't exceed CustomText limit
         *      2- Construct page without cutting through paragraphs
         *  Special cases: 
         *      1- Paragraph overflows to next page. 
         *          - Overflowing Paragraph will be added to next page
         *      2- Paragraph length bigger than character limit
         *          - Paragraph will be split into different chunks
         */
        private List<ChunkInfo> ChunkPages(MsReadParseResult parsingResult)
        {
            var resultPages = new List<ChunkInfo>();
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => l.BoundingBox[2] - l.BoundingBox[0]).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * Constants.MaxLineLengthPrecentile)] * Constants.PercentageOfMaxLineLength;
            var currentParagraph = new StringBuilder();
            StringBuilder pageText = new StringBuilder();
            var currentPage = 0;
            var totalPageCount = parsingResult.RecognitionResults.Count;
            var currentPageStart = 1;
            var currentParagraphPageStart = 1;
            // loop over each page to create list of pages
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                // update paragraphPageStart if no overflowing paragraph in new page
                currentParagraphPageStart = currentParagraph.Length > 0 ? currentParagraphPageStart : (int)rr.Page;
                foreach (Line l in rr.Lines)
                {
                    // Special case: paragraph length is bigger that character limit
                    if (currentParagraph.Length + l.Text.Length > Constants.CustomTextPredictionMaxCharLimit)
                    {
                        // add existing page to result
                        if (pageText.Length > 0)
                        {
                            resultPages.Add(new ChunkInfo(pageText.ToString(), currentPageStart, (int)rr.Page));
                            pageText.Clear();
                        }
                        // concatenate paragraph to current page
                        pageText.Append(currentParagraph.ToString());
                        currentParagraph.Clear();
                        currentPageStart = currentParagraphPageStart;
                    }
                    // concatenate line to current paragraph
                    currentParagraph.Append($"{l.Text} ");
                    // end of paragraph
                    if (IsLineEndOfParagraph(l, maxLineLength))
                    {
                        // if adding the paragraph to the page exceeds the character limit
                        // current page will be added to result and the paragraph will be added to the next page
                        if (pageText.Length + currentParagraph.Length > Constants.CustomTextPredictionMaxCharLimit)
                        {
                            var text = pageText.ToString().Trim();
                            var chunkInfo = new ChunkInfo(text, currentPageStart, (int)rr.Page);
                            resultPages.Add(chunkInfo);
                            pageText.Clear();
                            currentPageStart = (int)rr.Page;
                        }
                        // concatenate paragraph to current page
                        pageText.Append(currentParagraph.ToString());
                        currentParagraph.Clear();
                        currentParagraphPageStart = (int)rr.Page;
                    }
                }
                // special case: if last page add any text in the current paragraph to the page
                if (currentParagraph.Length > 0 && ++currentPage == totalPageCount)
                {
                    pageText.Append(currentParagraph.ToString());
                }
                // add pageText to list of pages after concatenating all paragraphs
                if (pageText.Length > 0)
                {
                    var text = pageText.ToString().Trim();
                    var chunkInfo = new ChunkInfo(text, currentPageStart, (int)rr.Page);
                    resultPages.Add(chunkInfo);
                    pageText.Clear();
                    currentPageStart = currentParagraph.Length > 0 ? (int)rr.Page : ((int)rr.Page + 1);
                }
            }
            return resultPages;
        }

        /*
         *  Data structure: 
         *      MsReadParseResult is a list of pages and each page contains a list of lines
         *  Intuition:
         *      To construct a chunk, we concatenate paragraphs to the chunk without exceeding the character limit
         *  Considerations:
         *      1- Chunk doesn't exceed character limit
         *      2- Construct chunk without cutting through paragraphs
         *  Special cases:
         *      1- Paragraph length bigger than character limit
         *          - Paragraph will be split into different chunks
         */
        private List<ChunkInfo> ChunkCharacters(MsReadParseResult parsingResult, int charLimit)
        {
            StringBuilder currentParagraph = new StringBuilder();
            StringBuilder currentChunk = new StringBuilder();
            List<ChunkInfo> resultChunks = new List<ChunkInfo>();
            double maxLineLength = CaluculateMaxLineLength(parsingResult);
            var currentChunkPageStart = 1;
            var currentParagraphPageStart = 1;
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                // update paragraphPageStart if no overflowing paragraph in new page
                currentParagraphPageStart = currentParagraph.Length > 0 ? currentParagraphPageStart : (int)rr.Page;
                foreach (Line l in rr.Lines)
                {
                    // Special case: paragraph length is bigger that character limit
                    if (currentParagraph.Length + l.Text.Length > Constants.CustomTextPredictionMaxCharLimit)
                    {
                        // add existing chunk to result
                        if (currentChunk.Length > 0)
                        {
                            resultChunks.Add(new ChunkInfo(currentChunk.ToString(), currentChunkPageStart, (int)rr.Page));
                            currentChunk.Clear();
                        }
                        // concatenate paragraph to current chunk
                        currentChunk.Append(currentParagraph.ToString());
                        currentParagraph.Clear();
                        currentChunkPageStart = currentParagraphPageStart;
                    }
                    // concatenate line to current paragraph
                    currentParagraph.Append($"{l.Text} ");
                    // end of paragraph
                    if (IsLineEndOfParagraph(l, maxLineLength))
                    {
                        // if adding the paragraph to the chunk exceeds the character limit
                        // current chunk will be added to result and the paragraph will be added to the next chunk
                        if (currentChunk.Length + currentParagraph.Length > charLimit)
                        {
                            var text = currentChunk.ToString().Trim();
                            var chunkInfo = new ChunkInfo(text, currentChunkPageStart, (int)rr.Page);
                            resultChunks.Add(chunkInfo);
                            currentChunk.Clear();
                            currentChunkPageStart = (int)rr.Page;
                        }
                        // concatenate paragraph to current chunk
                        currentChunk.Append(currentParagraph.ToString());
                        currentParagraph.Clear();
                    }
                }
            }
            // Add remaining text after loop ends
            if (currentParagraph.Length > 0 || currentChunk.Length > 0)
            {
                currentChunk.Append(currentParagraph.ToString());
                var text = currentChunk.ToString().Trim();
                var chunkInfo = new ChunkInfo(text, currentChunkPageStart, parsingResult.RecognitionResults.Count);
                resultChunks.Add(chunkInfo);
            }
            return resultChunks;
        }

        private static double CaluculateMaxLineLength(MsReadParseResult parsingResult)
        {
            /*
             * Line.BoundingBox is an array of coordinates for current line (as OCR detetced)
             * 
             *   [0, 1] ------------------ [2, 3]
             *         -                  -
             *         -                  -
             *         -                  -
             *   [4, 5] ------------------ [6, 7]
             */
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => l.BoundingBox[2] - l.BoundingBox[0]).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * Constants.MaxLineLengthPrecentile)] * Constants.PercentageOfMaxLineLength;
            return maxLineLength;
        }

        private bool IsLineEndOfParagraph(Line line, double maxLineLength)
        {
            return line.BoundingBox[2] - line.BoundingBox[0] < maxLineLength;
        }
        public List<ChunkInfo> Chunk(string text, int charLimit)
        {
            throw new NotImplementedException();
        }

        public void ValidateFileType(string fileName)
        { }
    }
}
