// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Parser
{
    /// <summary>
    ///   1) What this class does
    ///       - api
    ///           - Function
    ///               - ParseFile(Stream file)
    ///               - Returns: ParsedDocument
    ///       - calls MSRead SDk to parse given document
    ///       - then maps the result to our ParsedDocument model
    ///   
    ///   
    ///   2) MSRead Return Type
    ///       - list<pages>
    ///           - list<lines>
    ///               - BoundingBox
    ///   
    ///       - BoundingBox representation
    ///           - Line.BoundingBox is an array of coordinates for current line (as OCR detetced)
    ///              [0, 1] ------------------ [2, 3]
    ///                    -                  -
    ///                    -                  -
    ///                    -                  -
    ///              [4, 5] ------------------ [6, 7]
    ///   
    ///   
    ///   3) Mapping Logic
    ///   - map MSRead result to intermediary object
    ///       - ParsedDocument
    ///           - List<DocumentElement>
    ///               - DocumentElement
    ///                   - Text
    ///                   - Type (Title, Heading, Paragraph, Table, BulletPoints)
    ///   - we can only detect paragraphs
    ///   - our goal is to honor paragraphs and not cut through them2) Our target
    ///   
    ///   
    ///   4) pargraph extraction
    ///   - to construct paragraph
    ///     we keep on adding lines until we find ending line of the paragraph
    ///   
    ///   
    ///   5) Finding EndofParagarph heurestics
    ///   - case 1: next line spacing > previous line spacing
    ///       - nextLineSpacing > previousLineSpacing * EndOfParagraphVerticalSpaceFactor;
    ///       - EndOfParagraphVerticalSpaceFactor = arbitrary value (1.5)
    ///   - case 2: next line is indented
    ///       - Equation: X Co-ordinate of the top left bounding box of the next line > medianLineStart + indentLength
    ///       - medianLineStart = median x coordinate of lines' start
    ///       - indentLength
    ///           - is the 'min' indent length of every line
    ///           - sort lines according to length
    ///           - full line length = 95th percentile
    ///           - indentLength = 0.05 * (full line length)
    ///   - case 3: last line is smaller than the average line 
    ///       - GetBoundingBoxTopRightX(line) < (medianLineEnd - Constants.MaxNumberOfIndentsAfterLine * indentLength);
    /// </summary>
    public class MSReadParserService : IParserService
    {
        private ComputerVisionClient _client;
        private HashSet<string> _validTypesSet;

        public MSReadParserService(string cognitiveServiceEndPoint, string congnitiveServiceKey)
        {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(congnitiveServiceKey))
            { Endpoint = cognitiveServiceEndPoint };
            TestConnectionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            _validTypesSet = new HashSet<string>(Constants.MsReadValidFileTypes, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<DocumentTree> ParseFile(Stream file)
        {
            var result = await ParseFileInternal(file);
            return MapMsReadResult(result);
        }

        public async Task<ReadOperationResult> ParseFileInternal(Stream file)
        {
            var response = await _client.ReadInStreamAsync(file);
            const int NumberOfCharsInOperationId = 36;
            string operationId = response.OperationLocation.Substring(response.OperationLocation.Length - NumberOfCharsInOperationId);

            ReadOperationResult result;
            do
            {
                result = await _client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while (result.Status == OperationStatusCodes.Running || result.Status == OperationStatusCodes.NotStarted);
            return result;
        }

        /// <summary>
        /// Extract paragraphs from MsRead parse result using bounding box information.
        /// Paragraphs that span two pages are moved to the second page
        /// </summary>
        public DocumentTree MapMsReadResult(ReadOperationResult parsingResult)
        {
            var elements = new List<DocumentElement>();
            var medianLineStart = CalculateMedianLineStart(parsingResult);
            var medianLineEnd = CalculateMedianLineEnd(parsingResult);
            var indentLength = CalculateIndentLength(parsingResult);
            var currentParagraph = new StringBuilder();
            var currentPage = 0;
            int currentParagraphPageStart = 1;
            var totalPageCount = parsingResult.AnalyzeResult.ReadResults.Count;
            // loop over each page to create list of pages
            foreach (var rr in parsingResult.AnalyzeResult.ReadResults)
            {
                // update paragraphPageStart if no overflowing paragraph in new page
                currentParagraphPageStart = currentParagraph.Length > 0 ? currentParagraphPageStart : (int)rr.Page;
                // normal flow: loop over lines
                for (int i = 0; i < rr.Lines.Count; i++)
                {
                    Line l = rr.Lines[i];
                    Line nextLine = i < rr.Lines.Count - 1 ? rr.Lines[i + 1] : null;
                    Line previousLine = i > 0 ? rr.Lines[i - 1] : null;
                    HandleNewLine(currentParagraph, elements, (int)rr.Page, ref currentParagraphPageStart, l, previousLine, nextLine, indentLength, medianLineStart, medianLineEnd);
                }
                // special case: if last page add text in the current paragraph to the elements
                if (++currentPage == totalPageCount && currentParagraph.Length > 0)
                {
                    HandleEndOfParagraph(currentParagraph, elements, parsingResult.AnalyzeResult.ReadResults.Count, ref currentParagraphPageStart);
                }
            }

            // construct document tree
            return new DocumentTree
            {
                RootSegment = new DocumentSegment
                {
                    RootElement = new DocumentElement
                    {
                        Text = string.Empty,
                        Type = ElementType.Root
                    },
                    Children = elements.Select(docElement =>
                    {
                        return new DocumentSegment
                        {
                            RootElement = docElement,
                            Children = null
                        };
                    }).ToList()
                }
            };
        }


        private void HandleNewLine(StringBuilder currentParagraph, List<DocumentElement> elements, int currentPage, ref int currentParagraphPageStart, Line l, Line previousLine, Line nextLine, double indentLength, double medianLineStart, double medianLineEnd)
        {
            // concatenate line to current paragraph
            currentParagraph.Append($"{l.Text} ");
            // end of paragraph
            if (IsLineEndOfParagraph(l, previousLine, nextLine, indentLength, medianLineStart, medianLineEnd))
            {
                HandleEndOfParagraph(currentParagraph, elements, currentPage, ref currentParagraphPageStart);
            }
        }

        private static void HandleEndOfParagraph(StringBuilder currentParagraph, List<DocumentElement> elements, int currentPage, ref int currentParagraphPageStart)
        {
            elements.Add(new DocumentElement
            {
                Text = currentParagraph.ToString(),
                PageNumber = currentParagraphPageStart,
                Type = ElementType.Paragraph
            });
            currentParagraphPageStart = currentPage;
            currentParagraph.Clear();
        }

        private bool IsLineEndOfParagraph(Line line, Line previousLine, Line nextLine, double indentLength, double medianLineStart, double medianLineEnd)
        {
            // detect end of paragraph: line spacing
            var verticalSpaceEndOfLine = false;
            if (nextLine != null && previousLine != null)
            {
                var previousLineSpacing = Math.Abs(GetBoundingBoxTopLeftY(line) - GetBoundingBoxTopLeftY(previousLine));
                var nextLineSpacing = Math.Abs(GetBoundingBoxTopLeftY(line) - GetBoundingBoxTopLeftY(nextLine));
                verticalSpaceEndOfLine = nextLineSpacing > previousLineSpacing * Constants.EndOfParagraphVerticalSpaceFactor;
            }

            // detect end of paragraph: next line indentation
            var nextLineIndented = nextLine != null && GetBoundingBoxTopLeftX(nextLine) > medianLineStart + indentLength;

            // detect end of paragraph: current line length
            var lineLengthSmallerThanMinLine = GetBoundingBoxTopRightX(line) < (medianLineEnd - Constants.MaxNumberOfIndentsAfterLine * indentLength);

            // return condition
            return verticalSpaceEndOfLine || nextLineIndented || lineLengthSmallerThanMinLine;
        }

        private double CalculateIndentLength(ReadOperationResult parsingResult)
        {
            // sort lines by width
            var linesArray = parsingResult.AnalyzeResult.ReadResults.SelectMany(p => p.Lines).Select(l => GetBoundingBoxTopRightX(l) - GetBoundingBoxTopLeftX(l)).OrderBy(l => l).ToArray();
            return linesArray[(int)(linesArray.Length * Constants.MaxLineLengthPrecentile)] * Constants.IndentPercentageOfLine;
        }

        private double CalculateMedianLineStart(ReadOperationResult parsingResult)
        {
            var linesArraySortedByStart = parsingResult.AnalyzeResult.ReadResults.SelectMany(p => p.Lines).OrderBy(l => GetBoundingBoxTopLeftX(l)).ToArray();
            return GetBoundingBoxTopLeftX(linesArraySortedByStart[linesArraySortedByStart.Length / 2]);
        }

        private double CalculateMedianLineEnd(ReadOperationResult parsingResult)
        {
            // get the top-right x co-ordinate of each line's bounding box -> ascendingly
            var linesArraySortedByEnd = parsingResult.AnalyzeResult.ReadResults.SelectMany(p => p.Lines.Select(l => GetBoundingBoxTopRightX(l))).OrderBy(x => x).ToArray();
            // return the median element
            return linesArraySortedByEnd[linesArraySortedByEnd.Length / 2];
        }

        private double GetBoundingBoxTopLeftX(Line line)
        {
            return (double)line.BoundingBox[0];
        }

        private double GetBoundingBoxTopRightX(Line line)
        {
            return (double)line.BoundingBox[2];
        }

        private double GetBoundingBoxTopLeftY(Line line)
        {
            return (double)line.BoundingBox[1];
        }

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName), Constants.MsReadValidFileTypes);
            }
        }

        private async Task TestConnectionAsync()
        {
            try
            {
                var file = new MemoryStream();
                var response = await _client.ReadInStreamAsync(file);
            }
            catch (ComputerVisionErrorException e)
            {
                if (!e.Message.Contains("BadRequest"))
                {
                    throw new MsReadConnectionException(e.Message);
                }
            }
        }
    }
}
