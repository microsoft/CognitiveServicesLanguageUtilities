// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Parser
{
    public interface IParserPoolManager
    {
        public IParserService GetParser(string fileType, string fileName);
    }
}