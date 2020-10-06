using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Parser
{
    /// <summary>
    ///  This class parses documents to a list of paragraphs using MsRead API
    ///  Data structure: 
    ///      MsReadParseResult is a list of pages and each page contains a list of lines
    ///      Line.BoundingBox is an array of coordinates for current line (as OCR detetced)
    ///  
    ///           [0, 1] ------------------ [2, 3]
    ///                 -                  -
    ///                 -                  -
    ///                 -                  -
    ///           [4, 5] ------------------ [6, 7]
    /// 
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

        private async Task TestConnectionAsync()
        {
            try
            {
                var file = new MemoryStream();
                var response = await _client.BatchReadFileInStreamAsync(file);
            }
            catch (ComputerVisionErrorException e)
            {
                if (!e.Message.Contains("BadRequest"))
                {
                    throw new MsReadConnectionException(e.Message);
                }
            }
        }

        public async Task<ParsedDocument> ParseFile(Stream file)
        {
            var result = await ParseFileInternal(file);
            return MapMsReadResult(result);
        }

        public async Task<ReadOperationResult> ParseFileInternal(Stream file)
        {
            var response = await _client.BatchReadFileInStreamAsync(file);
            const int NumberOfCharsInOperationId = 36;
            string operationId = response.OperationLocation.Substring(response.OperationLocation.Length - NumberOfCharsInOperationId);

            ReadOperationResult result;
            do
            {
                result = await _client.GetReadOperationResultAsync(operationId);
            }
            while (result.Status == TextOperationStatusCodes.Running || result.Status == TextOperationStatusCodes.NotStarted);
            return result;
        }

        /// <summary>
        /// Extract paragraphs from MsRead parse result using bounding box information.
        /// Paragraphs that span two pages are moved to the second page
        /// </summary>
        public ParsedDocument MapMsReadResult(ReadOperationResult parsingResult)
        {
            var elements = new List<DocumentElement>();
            var medianLineStart = CalculateMedianLineStart(parsingResult);
            var medianLineEnd = CalculateMedianLineEnd(parsingResult);
            var indentLength = CalculateIndentLength(parsingResult);
            var currentParagraph = new StringBuilder();
            var currentPage = 0;
            int currentParagraphPageStart = 1;
            var totalPageCount = parsingResult.RecognitionResults.Count;
            // loop over each page to create list of pages
            foreach (var rr in parsingResult.RecognitionResults)
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
                    HandleEndOfParagraph(currentParagraph, elements, parsingResult.RecognitionResults.Count, ref currentParagraphPageStart);
                }
            }
            return new ParsedDocument
            {
                Elements = elements
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

        private double CalculateIndentLength(ReadOperationResult parsingResult)
        {
            var linesArray = parsingResult.RecognitionResults.SelectMany(p => p.Lines).Select(l => GetBoundingBoxTopRightX(l) - GetBoundingBoxTopLeftX(l)).OrderBy(l => l).ToArray();
            return linesArray[(int)(linesArray.Length * Constants.MaxLineLengthPrecentile)] * Constants.IndentPercentageOfLine;
        }

        private double CalculateMedianLineStart(ReadOperationResult parsingResult)
        {
            var linesArraySortedByStart = parsingResult.RecognitionResults.SelectMany(p => p.Lines).OrderBy(l => GetBoundingBoxTopLeftX(l)).ToArray();
            return GetBoundingBoxTopLeftX(linesArraySortedByStart[linesArraySortedByStart.Length / 2]);
        }

        private double CalculateMedianLineEnd(ReadOperationResult parsingResult)
        {
            var linesArraySortedByEnd = parsingResult.RecognitionResults.SelectMany(p => p.Lines).OrderBy(l => GetBoundingBoxTopRightX(l)).ToArray();
            return GetBoundingBoxTopRightX(linesArraySortedByEnd[linesArraySortedByEnd.Length / 2]);
        }

        private bool IsLineEndOfParagraph(Line line, Line previousLine, Line nextLine, double indentLength, double medianLineStart, double medianLineEnd)
        {
            var verticalSpaceEndOfLine = nextLine != null && previousLine != null && Math.Abs(GetBoundingBoxTopLeftY(line) - GetBoundingBoxTopLeftY(nextLine)) > Math.Abs(GetBoundingBoxTopLeftY(line) - GetBoundingBoxTopLeftY(previousLine)) * Constants.EndOfParagraphVerticalSpaceFactor;
            var nextLineIndented = nextLine != null && GetBoundingBoxTopLeftX(nextLine) > medianLineStart + indentLength;
            var lineLengthSmallerThanMinLine = GetBoundingBoxTopRightX(line) < (medianLineEnd - Constants.MaxNumberOfIndentsAfterLine * indentLength);
            return verticalSpaceEndOfLine || nextLineIndented || lineLengthSmallerThanMinLine;
        }

        private double GetBoundingBoxTopLeftX(Line line)
        {
            return line.BoundingBox[0];
        }

        private double GetBoundingBoxTopRightX(Line line)
        {
            return line.BoundingBox[2];
        }

        private double GetBoundingBoxTopLeftY(Line line)
        {
            return line.BoundingBox[1];
        }

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName), Constants.MsReadValidFileTypes);
            }
        }
    }
}
