using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Chunker
{
    /*
     *  Data structure: 
     *      MsReadParseResult is a list of pages and each page contains a list of lines
     *      Line.BoundingBox is an array of coordinates for current line (as OCR detetced)
     * 
     *           [0, 1] ------------------ [2, 3]
     *                 -                  -
     *                 -                  -
     *                 -                  -
     *           [4, 5] ------------------ [6, 7]
    */
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
                    return ChunkPages(msReadparsingResult, charLimit);
                default:
                    throw new NotSupportedException($"The chunk type {chunkMethod} isn't supported.");
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
                new ChunkInfo(1, text, 1, parsingResult.RecognitionResults.Count())
            };
        }

        /*
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
        private List<ChunkInfo> ChunkPages(MsReadParseResult parsingResult, int charLimit)
        {
            var resultPages = new List<ChunkInfo>();
            var maxLineLength = CaluculateMaxLineLength(parsingResult);
            var currentParagraph = new StringBuilder();
            StringBuilder pageText = new StringBuilder();
            var currentPage = 0;
            var totalPageCount = parsingResult.RecognitionResults.Count;
            var currentChunkPageStart = 1;
            var currentParagraphPageStart = 1;
            var chunkCounter = 1;
            // loop over each page to create list of pages
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                // update paragraphPageStart if no overflowing paragraph in new page
                currentParagraphPageStart = currentParagraph.Length > 0 ? currentParagraphPageStart : (int)rr.Page;
                foreach (Line l in rr.Lines)
                {
                    AppendLineToChunk(charLimit, currentParagraph, pageText, resultPages, maxLineLength, ref currentChunkPageStart, ref currentParagraphPageStart, ref chunkCounter, (int)rr.Page, l);
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
                    var chunkInfo = new ChunkInfo(chunkCounter++, text, currentChunkPageStart, (int)rr.Page);
                    resultPages.Add(chunkInfo);
                    pageText.Clear();
                    currentChunkPageStart = currentParagraph.Length > 0 ? (int)rr.Page : ((int)rr.Page + 1);
                }
            }
            return resultPages;
        }

        /*
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
            var maxLineLength = CaluculateMaxLineLength(parsingResult);
            var currentChunkPageStart = 1;
            var currentParagraphPageStart = 1;
            var chunkCounter = 1;
            foreach (TextRecognitionResult rr in parsingResult.RecognitionResults)
            {
                // update paragraphPageStart if no overflowing paragraph in new page
                currentParagraphPageStart = currentParagraph.Length > 0 ? currentParagraphPageStart : (int)rr.Page;
                foreach (Line l in rr.Lines)
                {
                    AppendLineToChunk(charLimit, currentParagraph, currentChunk, resultChunks, maxLineLength, ref currentChunkPageStart, ref currentParagraphPageStart, ref chunkCounter, (int)rr.Page, l);
                }
            }
            // Add remaining text after loop ends
            if (currentParagraph.Length > 0 || currentChunk.Length > 0)
            {
                currentChunk.Append(currentParagraph.ToString());
                var text = currentChunk.ToString().Trim();
                var chunkInfo = new ChunkInfo(chunkCounter++, text, currentChunkPageStart, parsingResult.RecognitionResults.Count);
                resultChunks.Add(chunkInfo);
            }
            return resultChunks;
        }

        private void AppendLineToChunk(int charLimit, StringBuilder currentParagraph, StringBuilder currentChunk, List<ChunkInfo> resultChunks, double maxLineLength, ref int currentChunkPageStart, ref int currentParagraphPageStart, ref int chunkCounter, int currentPage, Line l)
        {
            // Special case: paragraph length is bigger that character limit
            if (currentParagraph.Length + l.Text.Length > charLimit)
            {
                HandleParagraphLengthGreaterThanCharLimit(currentParagraph, currentChunk, resultChunks, ref currentChunkPageStart, ref currentParagraphPageStart, ref chunkCounter, currentPage);
            }
            // concatenate line to current paragraph
            currentParagraph.Append($"{l.Text} ");
            // end of paragraph
            if (IsLineEndOfParagraph(l, maxLineLength))
            {
                HandleEndOfParagraph(charLimit, currentParagraph, currentChunk, resultChunks, ref currentChunkPageStart, ref currentParagraphPageStart, ref chunkCounter, currentPage);
            }
        }

        private static void HandleEndOfParagraph(int charLimit, StringBuilder currentParagraph, StringBuilder currentChunk, List<ChunkInfo> resultChunks, ref int currentChunkPageStart, ref int currentParagraphPageStart, ref int chunkCounter, int currentPage)
        {
            // if adding the paragraph to the chunk exceeds the character limit
            // current chunk will be added to result and the paragraph will be added to the next chunk
            if (currentChunk.Length + currentParagraph.Length > charLimit)
            {
                var text = currentChunk.ToString().Trim();
                var chunkInfo = new ChunkInfo(chunkCounter++, text, currentChunkPageStart, currentParagraphPageStart);
                resultChunks.Add(chunkInfo);
                currentChunk.Clear();
                currentChunkPageStart = currentParagraphPageStart;
            }
            // concatenate paragraph to current chunk
            currentChunk.Append(currentParagraph.ToString());
            currentParagraph.Clear();
            currentParagraphPageStart = currentPage;
        }

        private static void HandleParagraphLengthGreaterThanCharLimit(StringBuilder currentParagraph, StringBuilder currentChunk, List<ChunkInfo> resultChunks, ref int currentChunkPageStart, ref int currentParagraphPageStart, ref int chunkCounter, int currentPage)
        {
            // add existing chunk to result
            if (currentChunk.Length > 0)
            {
                resultChunks.Add(new ChunkInfo(chunkCounter++, currentChunk.ToString(), currentChunkPageStart, currentParagraphPageStart));
                currentChunk.Clear();
            }
            // concatenate paragraph to current chunk
            currentChunk.Append(currentParagraph.ToString());
            currentParagraph.Clear();
            currentChunkPageStart = currentParagraphPageStart;
            currentParagraphPageStart = currentPage;
        }

        private double CaluculateMaxLineLength(MsReadParseResult parsingResult)
        {
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => GetBoundingBoxTopRightX(l) - GetBoundingBoxTopLeftX(l)).OrderBy(l => l).ToArray();
            var maxLineLength = linesArray[(int)(linesArray.Length * Constants.MaxLineLengthPrecentile)] * Constants.PercentageOfMaxLineLength;
            return maxLineLength;
        }

        private bool IsLineEndOfParagraph(Line line, double maxLineLength)
        {
            return GetBoundingBoxTopRightX(line) - GetBoundingBoxTopLeftX(line) < maxLineLength;
        }

        private double GetBoundingBoxTopLeftX(Line line)
        {
            return line.BoundingBox[0];
        }

        private double GetBoundingBoxTopRightX(Line line)
        {
            return line.BoundingBox[2];
        }

        public List<ChunkInfo> Chunk(string text, int charLimit)
        {
            throw new NotImplementedException();
        }

        public void ValidateFileType(string fileName)
        { }
    }
}