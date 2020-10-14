using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Chunker
{
    public class ChunkerService : IChunkerService
    {
        
        private readonly string _primaryDelimiter = ".";
        private readonly string _secondaryDelimiter = " ";

        public List<ChunkInfo> Chunk(DocumentTree documentTree, ChunkMethod chunkMethod, int charLimit)
        {
            switch (chunkMethod)
            {
                case ChunkMethod.NoChunking:
                    return ApplyNoChunking(documentTree);
                case ChunkMethod.Char:
                    return ChunkByCharacterLimit(documentTree, charLimit);
                case ChunkMethod.Page:
                    return ChunkByPage(documentTree, charLimit);
                default:
                    throw new NotSupportedException($"The chunk type {chunkMethod} isn't supported.");
            }
        }

        public List<ChunkInfo> Chunk(string text, int charLimit)
        {
            throw new NotImplementedException();
        }

        public void ValidateFileType(string fileName)
        { }

        /*
         *  Join all elements into a single string
         */
        private List<ChunkInfo> ApplyNoChunking(DocumentTree documentTree)
        {
            var resultText = new StringBuilder();
            foreach (var docSegment in documentTree.DocumentSegments)
            {
                var segmentText = ApplyNoChunkingInternal(docSegment);
                resultText.Append(segmentText);
            }
            var text = resultText.ToString().Trim();
            var firstPage = documentTree.DocumentSegments.FirstOrDefault()?.RootElement.PageNumber;
            var lastPage = documentTree.DocumentSegments.LastOrDefault()?.RootElement.PageNumber;
            return new List<ChunkInfo>
            {
                new ChunkInfo(chunkNumber: 1, text, firstPage, lastPage)
            };
        }

        private string ApplyNoChunkingInternal(DocumentSegment documentSegment)
        {
            var finalText = new StringBuilder();
            // get root text
            var rootText = documentSegment.RootElement.Text;
            finalText.Append(rootText);
            finalText.Append(Environment.NewLine);
            // get children text
            if (documentSegment.Children != null)
            {
                foreach (var childSegment in documentSegment.Children)
                {
                    var segmentText = ApplyNoChunkingInternal(childSegment);
                    finalText.Append(segmentText);
                }
            }
            return finalText.ToString();
        }

        /*
         *  Intuition:
         *      To construct a page, we concatenate elements in the same page to the chunk without exceeding the character limit
         *  Considerations:
         *      1- Chunk doesn't exceed character limit
         *  Special cases:
         *      1- Element length bigger than character limit
         *          - Element will be split into different chunks
         */
        private List<ChunkInfo> ChunkByPage(DocumentTree documentTree, int charLimit)
        {
            var pages = new List<ChunkInfo>();
            var currentChunkNumber = 1;
            var currentPageNumber = documentTree.Elements.FirstOrDefault()?.PageNumber;
            var currentChunk = new StringBuilder();
            documentTree.Elements.ForEach(e =>
            {
                if ((e.PageNumber != currentPageNumber || e.Text.Length + currentChunk.Length > charLimit) && currentChunk.Length > 0)
                {
                    pages.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), currentPageNumber, currentPageNumber));
                    currentPageNumber = e.PageNumber;
                    currentChunkNumber++;
                    currentChunk.Clear();
                }
                if (e.Text.Length > charLimit)
                {
                    HandleParagraphLengthGreaterThanCharLimit(e.Text, charLimit, ref currentChunkNumber, pages, currentPageNumber);
                }
                else
                {
                    currentChunk.Append(e.Text);
                    currentChunk.Append(Environment.NewLine);
                }
            });
            // handle last page
            if (currentChunk.Length > 0)
            {
                pages.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), currentPageNumber, currentPageNumber));
            }
            return pages;
        }

        /*
         *  Intuition:
         *      To construct a chunk, we concatenate elements to the chunk without exceeding the character limit
         *  Considerations:
         *      1- Chunk doesn't exceed character limit
         *  Special cases:
         *      1- Element length bigger than character limit
         *          - Element will be split into different chunks
         */
        private List<ChunkInfo> ChunkByCharacterLimit(DocumentTree documentTree, int charLimit)
        {
            var characterChunks = new List<ChunkInfo>();
            var currentChunkNumber = 1;
            var chunkStartPage = documentTree.Elements.FirstOrDefault()?.PageNumber;
            var chunkEndPage = documentTree.Elements.FirstOrDefault()?.PageNumber;
            var currentChunk = new StringBuilder();
            documentTree.Elements.ForEach(e =>
            {
                if ((e.Text.Length + currentChunk.Length > charLimit) && currentChunk.Length > 0)
                {
                    characterChunks.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), chunkStartPage, chunkEndPage));
                    currentChunkNumber++;
                    chunkStartPage = e.PageNumber;
                    currentChunk.Clear();
                }
                chunkEndPage = e.PageNumber;
                if (e.Text.Length > charLimit)
                {
                    HandleParagraphLengthGreaterThanCharLimit(e.Text, charLimit, ref currentChunkNumber, characterChunks, e.PageNumber);
                }
                else
                {
                    currentChunk.Append(e.Text);
                    currentChunk.Append(Environment.NewLine);
                }
            });
            // handle remaining text
            if (currentChunk.Length > 0)
            {
                characterChunks.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), chunkStartPage, chunkEndPage));
            }
            return characterChunks;
        }

        private void HandleParagraphLengthGreaterThanCharLimit(string paragraphText, int charLimit, ref int currentChunkNumber, List<ChunkInfo> chunks, int? pageNumber)
        {
            var blocks = SplitTextToBlocks(paragraphText, charLimit, delimiter: _primaryDelimiter);
            foreach (var block in blocks)
            {
                chunks.Add(new ChunkInfo(currentChunkNumber++, block, pageNumber, pageNumber));
            }
        }

        /// <summary>
        /// This method is called when the paragraph itself exceeds the character limit
        /// so we need to properly split the paragraph
        /// first: we split the text by _primaryDelimiter = "."
        /// and if any of the resulting sentences exceeds char limit, we split that sentence by _secondaryDelimiter = " "
        /// and if any of the resulting words exceeds the char limit, we split by char length (get substring)
        /// </summary>
        /// <param name="paragraphText"></param>
        /// <param name="charLimit"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        private List<string> SplitTextToBlocks(string paragraphText, int charLimit, string delimiter)
        {
            var blocks = new List<string>();
            var lines = paragraphText.Split(delimiter);
            var currentBlock = new StringBuilder();
            foreach (var line in lines)
            {
                if (line.Length + currentBlock.Length > charLimit)
                {
                    blocks.Add(currentBlock.ToString());
                    currentBlock.Clear();
                }
                if (line.Length > charLimit)
                {
                    // handle infinite loop
                    if (delimiter == _secondaryDelimiter)
                    {
                        var blocksByLength = SplitStringByLength(line, charLimit);
                        blocks.AddRange(blocksByLength);
                    }
                    var blocksByWords = SplitTextToBlocks(line, charLimit, _secondaryDelimiter);
                    blocks.AddRange(blocksByWords);
                }
                else
                {
                    currentBlock.Append(line);
                    currentBlock.Append(delimiter);
                }
            }
            if (currentBlock.Length > 0)
            {
                blocks.Add(currentBlock.ToString());
            }
            return blocks;
        }

        private List<string> SplitStringByLength(string str, int length)
        {
            if (string.IsNullOrEmpty(str) || length < 1)
            {
                return new List<string> { str };
            }
            return Enumerable.Range(0, str.Length / length)
                            .Select(i => str.Substring(i * length, length)).ToList();
        }
    }
}