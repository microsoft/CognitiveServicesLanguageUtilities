// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using Microsoft.CogSLanguageUtilities.Tests.Configs;
using Microsoft.CogSLanguageUtilities.Tests.TestUtils;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.UnitTests.ApplicationLayer.Services.Parser
{
    public class MsReadParserServiceUnitTest
    {
        private MSReadParserService _parser;

        public MsReadParserServiceUnitTest()
        {
            _parser = new MSReadParserService(Secrets.MSReadCognitiveServicesEndPoint, Secrets.MSReadCongnitiveServicesKey);
        }

        public static TheoryData MapMsReadParserServiceTestData()
        {
            var inputString = File.ReadAllText(@"TestData\Parser\MSRead\MapMsReadTestInput.json");
            var input = JsonConvert.DeserializeObject<ReadOperationResult>(inputString);
            var expectedString = File.ReadAllText(@"TestData\Parser\MSRead\MapMsReadTestOutput.json");
            var expected = JsonConvert.DeserializeObject<DocumentTree>(expectedString);
            return new TheoryData<ReadOperationResult, DocumentTree>
            {
                {
                    input,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(MapMsReadParserServiceTestData))]
        public void MapMsReadParserServiceTest(ReadOperationResult parsingResult, DocumentTree expected)
        {
            var actual = _parser.MapMsReadResult(parsingResult);
            Assert.Equal(expected, actual, new DocumentTreeComparer());
        }
    }
}
