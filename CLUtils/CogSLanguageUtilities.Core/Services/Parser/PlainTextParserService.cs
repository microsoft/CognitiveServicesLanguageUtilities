// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Parser
{
    public class PlainTextParserService : IParserService
    {
        private HashSet<string> _validTypesSet = new HashSet<string>(Constants.PlainTextValidFileTypes, StringComparer.OrdinalIgnoreCase);

        public async Task<ParsedDocument> ParseFile(Stream file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                var text = await sr.ReadToEndAsync();
                var element = new DocumentElement
                {
                    Text = text,
                    Type = ElementType.Paragraph
                };
                var elements = new List<DocumentElement> { element };
                return new ParsedDocument
                {
                    Elements = elements
                };
            }
        }

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName), Constants.PlainTextValidFileTypes);
            }
        }
    }
}
