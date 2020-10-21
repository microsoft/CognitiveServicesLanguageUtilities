// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using Microsoft.CogSLanguageUtilities.Tests.Configs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var expected = JsonConvert.DeserializeObject<ParsedDocument>(expectedString);
            return new TheoryData<ReadOperationResult, ParsedDocument>
            {
                {
                    input,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(MapMsReadParserServiceTestData))]
        public void MapMsReadParserServiceTest(ReadOperationResult parsingResult, ParsedDocument expected)
        {
            var actual = _parser.MapMsReadResult(parsingResult);
            Assert.Equal(expected, actual, new ParsedDocumentComparer());
        }

        public class ParsedDocumentComparer : IEqualityComparer<ParsedDocument>
        {
            public bool Equals(ParsedDocument x, ParsedDocument y)
            {
                var list = x.Elements.Zip(y.Elements, (e1, e2) => new { e1, e2 });
                foreach (var entry in list)
                {
                    if (entry.e1.PageNumber != entry.e2.PageNumber || entry.e1.Text != entry.e2.Text)
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(ParsedDocument obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
