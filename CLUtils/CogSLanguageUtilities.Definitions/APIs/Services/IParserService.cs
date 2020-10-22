// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IParserService
    {
        public Task<DocumentTree> ParseFile(Stream file);

        public void ValidateFileType(string fileType);
    }
}
