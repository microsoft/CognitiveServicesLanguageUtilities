

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Parser
{
    class DocxParserService : IParserService
    {
        private HashSet<string> _validTypesSet = new HashSet<string>(Constants.OpenXMLValidFileTypes, StringComparer.OrdinalIgnoreCase);
        public Task<ParsedDocument> ParseFile(Stream file)
        {
            // result object
            var resultElements = new List<DocumentElement>();

            // open a Wordprocessing document for editing.
            using (var wordDoc = WordprocessingDocument.Open(file, false))
            {
                // traverse through elements in the document
                var docElements = wordDoc.MainDocumentPart.Document.Body.ChildElements;
                foreach (var e in docElements)
                {
                    // skip empty elements
                    if (string.IsNullOrEmpty(e.InnerText)) { continue; }

                    // create new element
                    var newElement = new DocumentElement
                    {
                        Text = e.InnerText
                    };

                    // handle type cases
                    // case 1: element is a bullet point

                    // case 2: element is a table
                    if (e is Table)
                    {
                        newElement.Text = HandleTableElement(e as Table);
                        newElement.Type = Definitions.Enums.Parser.ElementType.Table;
                    }
                    else if (e is Paragraph)
                    {
                        // case 3: element is a title
                        if (e.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val == "Title"))
                        {
                            newElement.Type = Definitions.Enums.Parser.ElementType.Title;
                        }
                        // case 3: element is a headings
                        else if (e.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val == "Heading1"))
                        {
                            newElement.Type = Definitions.Enums.Parser.ElementType.Heading;
                        }
                        // case 4: element is a usual paragraph
                        else
                        {
                            newElement.Type = Definitions.Enums.Parser.ElementType.Paragraph;
                        }
                    }

                    // add element to list
                    resultElements.Add(newElement);
                }
            }
            return Task.FromResult(
                new ParsedDocument
                {
                    Elements = resultElements
                });
        }

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName), _validTypesSet.ToArray());
            }
        }

        private static string HandleTableElement(Table table)
        {
            var rows =
                table.ChildElements.Where(e => e is TableRow)
                .Select(row =>
                {
                    var cells = row.ChildElements.Where(e2 => e2 is TableCell).Select(cell => cell.InnerText);
                    return string.Join(" ", cells);
                });
            return string.Join(Environment.NewLine, rows);
        }
    }
}
