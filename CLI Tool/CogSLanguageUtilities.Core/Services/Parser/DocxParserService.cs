

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
    class DocxParserService : IParserService
    {
        private HashSet<string> _validTypesSet = new HashSet<string>(Constants.OpenXMLValidFileTypes, StringComparer.OrdinalIgnoreCase);

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName), _validTypesSet.ToArray());
            }
        }

        public Task<DocumentTree> ParseFile(Stream file)
        {
            // open a Wordprocessing document for editing.
            using (var wordDoc = WordprocessingDocument.Open(file, false))
            {
                // traverse through elements in the document
                var docElements = wordDoc.MainDocumentPart.Document.Body.ChildElements;

                // get document elements
                var currentIndex = 0;
                var currentElementType = ElementType.Root;
                var children = GetNestedChildren(docElements, ref currentIndex, currentElementType);
                return Task.FromResult(
                    new DocumentTree
                    {
                        RootSegment = new DocumentSegment
                        {
                            Children = children,
                            RootElement = new DocumentElement
                            {
                                Type = currentElementType
                            }
                        }
                    });
            }
        }

        private List<DocumentSegment> GetNestedChildren(OpenXmlElementList docElements, ref int currentIndex, ElementType parentType)
        {
            /*
             * function logic
             * loop over subsequent elements
             *      - while detected element precedence < parent element
             * if element of simple type (paragraph, table, bulleted list)
             *      - append to list
             * else
             *      - call function recursively to get children
             * return result list
             */
            if (currentIndex >= docElements.Count) { return null; }
            var result = new List<DocumentSegment>();

            // loop over elements
            var currentElement = docElements[currentIndex];
            var currentElementType = GetElementType(currentElement);
            while (IsLowerPrecedence(parentType, currentElementType))
            {
                // skip unhadled elements (charts, images, ..) or empty paragraphs
                if (currentElementType != ElementType.Other && !string.IsNullOrEmpty(currentElement.InnerText))
                {
                    var currentElementText = GetElementText(docElements, ref currentIndex);
                    // check if current element is of simple type
                    List<DocumentSegment> children = null;
                    if (!IsSimpleTypeElement(currentElementType))
                    {
                        // element is not simple type: i.e. can have nested children
                        currentIndex++; // index of subsequent element
                        children = GetNestedChildren(docElements, ref currentIndex, currentElementType);
                    }
                    // append element to result
                    var newSegment = new DocumentSegment
                    {
                        Children = children,
                        RootElement = new DocumentElement
                        {
                            Text = currentElementText,
                            Type = currentElementType
                        }
                    };
                    result.Add(newSegment);
                }

                // update next element
                currentIndex++;
                if (currentIndex >= docElements.Count) { break; }
                currentElement = docElements[currentIndex];
                currentElementType = GetElementType(currentElement);
            }
            return result;
        }

        private string GetElementText(OpenXmlElementList docElements, ref int currentElementIndex)
        {
            var currentElement = docElements[currentElementIndex];
            var currentElementType = GetElementType(currentElement);
            switch (currentElementType)
            {
                case ElementType.Table:
                    return HandleTableElement(currentElement as Table);
                case ElementType.BulletedList:
                    var bulletpointId = GetBulletpointId(currentElement);
                    return HandleBulletPoints(docElements, ref currentElementIndex, bulletpointId);
                default:
                    return currentElement.InnerText;
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

        private static string HandleBulletPoints(OpenXmlElementList docElements, ref int currentIndex, int bulletPointsId)
        {
            var textResult = new List<string>();
            var currElement = docElements[currentIndex];
            int? currElementNumberingId = bulletPointsId;
            while (currElement is Paragraph && currElementNumberingId == bulletPointsId && currentIndex < docElements.Count)
            {
                // add previous element
                textResult.Add(currElement.InnerText);

                // handle next element
                currElement = docElements[++currentIndex];
                var tmp = currElement.Elements<ParagraphProperties>()?.FirstOrDefault()?
                    .Elements<NumberingProperties>()?.FirstOrDefault()?
                    .Elements<NumberingId>()?.FirstOrDefault()?
                    .Val;
                currElementNumberingId = tmp == null ? null : (int?)tmp;
            }
            currentIndex--; // correct indexing
            return string.Join(Environment.NewLine, textResult);
        }


        private ElementType GetElementType(OpenXmlElement docElement)
        {
            if (docElement is Table)
            {
                return ElementType.Table;
            }
            else if (docElement is Paragraph)
            {
                var bulletPointsId = GetBulletpointId(docElement);

                // case 2: element is a title
                if (docElement.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val == "Title"))
                {
                    return ElementType.Title;
                }
                // case 3: element is a headings
                else if (docElement.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val == "Heading1"))
                {
                    return ElementType.Heading1;
                }
                else if (docElement.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val == "Heading2"))
                {
                    return ElementType.Heading2;
                }
                else if (docElement.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val == "Heading3"))
                {
                    return ElementType.Heading3;
                }
                // case 4: element is bullet points
                else if (bulletPointsId != null && docElement.Elements<ParagraphProperties>().Any(p => p.ParagraphStyleId.Val.ToString().IndexOf("heading", StringComparison.OrdinalIgnoreCase) >= 0) == false)
                {
                    /*
                     * TODO:
                     * are there any other 'paragraph' elements in the open xml format that has the 'NumberId' property?
                     * other than paragraphs which contain 'heading' attribute (which are already handled)
                     * if so we should add this to the 'if' condition
                     * in order to detect bulletpoints only
                     */
                    return ElementType.BulletedList;
                }
                // case 4: element is a usual paragraph
                else
                {
                    return ElementType.Paragraph;
                }
            }

            // for unknown elements
            return ElementType.Other;
        }

        private static Int32Value GetBulletpointId(OpenXmlElement docElement)
        {
            // bulletpoints detection
            return docElement.Elements<ParagraphProperties>()?.FirstOrDefault()?
                    .Elements<NumberingProperties>()?.FirstOrDefault()?
                    .Elements<NumberingId>()?.FirstOrDefault()?
                    .Val;
        }

        private bool IsSimpleTypeElement(ElementType elementType)
        {
            /*
             * by simple types we mean elements that can't have children
             * paragraphs, tables, bulleted lists
             */
            if (elementType == ElementType.Paragraph || elementType == ElementType.BulletedList || elementType == ElementType.Table)
            {
                return true;
            }
            return false;
        }

        private bool IsLowerPrecedence(ElementType baseElement, ElementType newElement)
        {
            /* 
             * returns true if precedence (newElement  < baseElement)
             */
            return newElement > baseElement;
        }
    }
}
