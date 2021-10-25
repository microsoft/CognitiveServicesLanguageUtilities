// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Chunker
{
    public class ChunkerService : IChunkerService
    {

        private readonly string _primaryDelimiter = ".";
        private readonly string _secondaryDelimiter = " ";

        public List<ChunkInfo> Chunk(DocumentTree documentTree, ChunkMethod chunkMethod, int charLimit, ElementType chunkLevel)
        {
            switch (chunkMethod)
            {
                case ChunkMethod.NoChunking:
                    return ApplyNoChunking(documentTree);
                case ChunkMethod.Char:
                    return ChunkByCharacterLimit(documentTree, charLimit);
                case ChunkMethod.Page:
                    return ChunkByPage(documentTree, charLimit);
                case ChunkMethod.Section:
                    return ChunkBySection(documentTree, chunkLevel, charLimit);
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
            foreach (var docSegment in documentTree.RootSegment.Children)
            {
                var segmentText = ApplyNoChunkingInternal(docSegment);
                resultText.Append(segmentText);
            }
            var text = resultText.ToString().Trim();
            var firstPage = documentTree.RootSegment.Children.FirstOrDefault()?.RootElement.PageNumber;
            var lastPage = documentTree.RootSegment.Children.LastOrDefault()?.RootElement.PageNumber;
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
            // prepare variables
            var pages = new List<ChunkInfo>();
            var currentChunkNumber = 1;
            var currentPageNumber = documentTree.RootSegment.Children.FirstOrDefault()?.RootElement.PageNumber;
            var currentChunk = new StringBuilder();

            // chunk document segments
            documentTree.RootSegment.Children.ForEach(segment =>
            {
                ChunkByPageInternal(segment, charLimit, pages, ref currentChunkNumber, ref currentPageNumber, currentChunk);
            });

            // handle last page
            if (currentChunk.Length > 0)
            {
                pages.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), currentPageNumber, currentPageNumber));
            }
            return pages;
        }

        private void ChunkByPageInternal(DocumentSegment segment, int charLimit, List<ChunkInfo> resultPages, ref int currentChunkNumber, ref int? currentPageNumber, StringBuilder currentChunkText)
        {
            // case 1: handle new page or page overflows char limit
            if ((segment.RootElement.PageNumber != currentPageNumber || segment.RootElement.Text.Length + currentChunkText.Length > charLimit) && currentChunkText.Length > 0)
            {
                resultPages.Add(new ChunkInfo(currentChunkNumber, currentChunkText.ToString(), currentPageNumber, currentPageNumber));
                currentPageNumber = segment.RootElement.PageNumber;
                currentChunkNumber++;
                currentChunkText.Clear();
            }

            // case 2: handle current segment text > char limit
            if (segment.RootElement.Text.Length > charLimit)
            {
                HandleParagraphLengthGreaterThanCharLimit(segment.RootElement.Text, charLimit, ref currentChunkNumber, resultPages, currentPageNumber);
            }

            // case 3: current element can be added to current chunk/page
            else
            {
                currentChunkText.Append(segment.RootElement.Text);
                currentChunkText.Append(Environment.NewLine);
            }

            // handle child segments
            if (segment.Children != null)
            {
                foreach (var childSegment in segment.Children)
                {
                    ChunkByPageInternal(childSegment, charLimit, resultPages, ref currentChunkNumber, ref currentPageNumber, currentChunkText);
                }
            }
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
            // prepare variables
            var resultChunks = new List<ChunkInfo>();
            var currentChunkNumber = 1;
            var chunkStartPage = documentTree.RootSegment.Children.FirstOrDefault()?.RootElement.PageNumber;
            var chunkEndPage = documentTree.RootSegment.Children.FirstOrDefault()?.RootElement.PageNumber;
            var currentChunk = new StringBuilder();

            // handle document segments
            documentTree.RootSegment.Children.ForEach(segment =>
            {
                ChunkByCharacterLimitInternal(segment, charLimit, resultChunks, ref currentChunkNumber, ref chunkStartPage, ref chunkEndPage, currentChunk);
            });

            // handle remaining text
            if (currentChunk.Length > 0)
            {
                resultChunks.Add(new ChunkInfo(currentChunkNumber, currentChunk.ToString(), chunkStartPage, chunkEndPage));
            }
            return resultChunks;
        }

        private void ChunkByCharacterLimitInternal(DocumentSegment segment, int charLimit, List<ChunkInfo> resultChunks, ref int currentChunkNumber, ref int? currentChunkStartPage, ref int? currentChunkEndPage, StringBuilder currentChunkText)
        {
            // case 1: handle chunk reached char limit
            if ((segment.RootElement.Text.Length + currentChunkText.Length > charLimit) && currentChunkText.Length > 0)
            {
                resultChunks.Add(new ChunkInfo(currentChunkNumber, currentChunkText.ToString(), currentChunkStartPage, currentChunkEndPage));
                currentChunkNumber++;
                currentChunkStartPage = segment.RootElement.PageNumber;
                currentChunkText.Clear();
            }
            currentChunkEndPage = segment.RootElement.PageNumber;

            // case 2: handle current element text > char limit
            if (segment.RootElement.Text.Length > charLimit)
            {
                HandleParagraphLengthGreaterThanCharLimit(segment.RootElement.Text, charLimit, ref currentChunkNumber, resultChunks, segment.RootElement.PageNumber);
            }

            // case 3: current element can be added to current chunk
            else
            {
                currentChunkText.Append(segment.RootElement.Text);
                currentChunkText.Append(Environment.NewLine);
            }
            // handle child segments
            if (segment.Children != null)
            {
                foreach (var childSegment in segment.Children)
                {
                    ChunkByCharacterLimitInternal(childSegment, charLimit, resultChunks, ref currentChunkNumber, ref currentChunkStartPage, ref currentChunkEndPage, currentChunkText);
                }
            }
        }

        private List<ChunkInfo> ChunkBySection(DocumentTree documentTree, ElementType chunkLevel, int charLimit)
        {
            var resultChunks = new List<ChunkInfo>();
            var currentChunkText = new StringBuilder();
            var canEndChunk = false; // true if current chunk contains a simple element
            var currentChunkNumber = 1;
            var currentChunkStartPage = documentTree.RootSegment.Children.FirstOrDefault()?.RootElement.PageNumber;
            var currentChunkEndPage = documentTree.RootSegment.Children.FirstOrDefault()?.RootElement.PageNumber;
            ChunkBySectionInternal(documentTree.RootSegment, currentChunkText, resultChunks, ref canEndChunk, chunkLevel, charLimit, ref currentChunkNumber, ref currentChunkStartPage, ref currentChunkEndPage);

            // handle remaining text
            if (currentChunkText.Length > 0)
            {
                resultChunks.Add(new ChunkInfo(currentChunkNumber, currentChunkText.ToString(), currentChunkStartPage, currentChunkEndPage));
            }
            return resultChunks;
        }

        private void ChunkBySectionInternal(DocumentSegment currentSegment, StringBuilder currentChunkText, List<ChunkInfo> resultChunks, ref bool canEndChunk, ElementType chunkLevel, int charLimit, ref int currentChunkNumber, ref int? currentChunkStartPage, ref int? currentChunkEndPage)
        {
            // End chunk If
            // current chunk contains a simple element and the current element is of the same or higher level than chunkLevel
            // or adding current element to the chunk exceeds charLimit
            var endChunkCondition = canEndChunk && currentSegment.RootElement.Type.IsHigherOrEqualPrecedence(chunkLevel) || (currentSegment.RootElement.Text.Length + currentChunkText.Length > charLimit);
            if (endChunkCondition)
            {
                resultChunks.Add(new ChunkInfo(currentChunkNumber, currentChunkText.ToString(), currentChunkStartPage, currentChunkEndPage));
                currentChunkText.Clear();
                currentChunkStartPage = currentSegment.RootElement.PageNumber;
                currentChunkNumber++;
            }
            currentChunkText.Append(currentSegment.RootElement.Text);
            currentChunkText.Append(Environment.NewLine);
            canEndChunk = currentSegment.RootElement.Type.IsSimpleTypeElement();
            currentChunkEndPage = currentSegment.RootElement.PageNumber;

            // DFS traversal of children 
            if (currentSegment.Children != null)
            {
                foreach (var childSegment in currentSegment.Children)
                {
                    ChunkBySectionInternal(childSegment, currentChunkText, resultChunks, ref canEndChunk, chunkLevel, charLimit, ref currentChunkNumber, ref currentChunkStartPage, ref currentChunkEndPage);
                }
            }
        }

        private void HandleParagraphLengthGreaterThanCharLimit(string paragraphText, int charLimit, ref int currentChunkNumber, List<ChunkInfo> resultChunks, int? pageNumber)
        {
            var blocks = SplitTextToBlocks(paragraphText, charLimit, delimiter: _primaryDelimiter);
            foreach (var block in blocks)
            {
                resultChunks.Add(new ChunkInfo(currentChunkNumber++, block, pageNumber, pageNumber));
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