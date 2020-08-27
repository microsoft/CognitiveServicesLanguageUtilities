using Xunit;
using CustomTextCliUtils.ApplicationLayer.Services.Parser;
using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Exceptions.Parser;
using System;
using CustomTextCliUtils.Tests.Configs;
using System.IO;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.Parser
{
    public class MSReadParserServiceTest
    {
        // Test Connection to MSRead Cognitive Service
        // ######################################################################
        public static TheoryData TestConnectionData()
        {
            // test 1 data
            string test1CognitiveServicesKey = Secrets.MSReadCongnitiveServiceKey;
            string test1CognitiveServicesEndpoint = Secrets.MSReadCognitiveServiceEndPoint;
            // test 2 data - wrong service key
            string test2Message = "";
            string test2CognitiveServicesKey = "2e10bb66ed3e7685bd3ca1b3abcd8f6b";
            string test2CognitiveServicesEndpoint = Secrets.MSReadCognitiveServiceEndPoint;
            // test 3 data - wrong endpoint
            string test3Message = "";
            string test3CognitiveServicesKey = Secrets.MSReadCongnitiveServiceKey;
            string test3CognitiveServicesEndpoint = "https://westus.api.cognitive.microsoft.com/";

            return new TheoryData<string, string, CliException>
            {
                {
                    // correct data 
                    test1CognitiveServicesKey,
                    test1CognitiveServicesEndpoint,
                    null
                },
                {
                    // wrong service key
                    test2CognitiveServicesKey,
                    test2CognitiveServicesEndpoint,
                    new MsReadConnectionException(test2Message, test2CognitiveServicesKey, test2CognitiveServicesEndpoint)
                },
                {
                    // wrong endpoint
                    test3CognitiveServicesKey,
                    test3CognitiveServicesEndpoint,
                    new MsReadConnectionException(test3Message, test3CognitiveServicesKey, test3CognitiveServicesEndpoint)
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestConnectionData))]
        public void TestConnection(string congnitiveServiceKey, string cognitiveServiceEndPoint, CliException expectedException)
        {
            if (expectedException == null)
            {
                new MSReadParserService(cognitiveServiceEndPoint, congnitiveServiceKey);
            }
            else {
                Assert.Throws(expectedException.GetType(), () => {
                    new MSReadParserService(cognitiveServiceEndPoint, congnitiveServiceKey);
                });
            }
        }


        // Test Connection to MSRead Cognitive Service
        // ######################################################################
        public static TheoryData TestParsingData()
        {
            // test 1 data
            var inputDocument = File.OpenRead(@"TestData\Parser\MSRead\test1 - inputDocument.pdf");  // read input document
            var parser = new MSReadParserService(Secrets.MSReadCognitiveServiceEndPoint, Secrets.MSReadCongnitiveServiceKey);
            var expectedResultFile = File.ReadAllText(@"TestData\Parser\MSRead\test1 - expectedResult.json");
            var expectedResult = JsonConvert.DeserializeObject<MsReadParseResult>(expectedResultFile);

            return new TheoryData<Stream, MSReadParserService, MsReadParseResult, CliException>
            {
                {
                    inputDocument,
                    parser,
                    expectedResult,
                    null
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestParsingData))]
        public void TestParsing(Stream inputDocument, MSReadParserService parser, MsReadParseResult expectedResult, CliException expectedException)
        {
            if (expectedException == null)
            {
                var tmp = parser.ParseFile(inputDocument).ConfigureAwait(false).GetAwaiter().GetResult();
                var actualResult = (MsReadParseResult)tmp;
                Assert.Equal(actualResult, expectedResult, new MSReadResultComparator());
            }
            else
            {
                Assert.Throws(expectedException.GetType(), () => {
                    parser.ParseFile(inputDocument).ConfigureAwait(false).GetAwaiter().GetResult();
                });
            }
        }
    }

    public class MSReadResultComparator : IEqualityComparer<MsReadParseResult>
    {
        public bool Equals(MsReadParseResult x, MsReadParseResult y)
        {
            var xLines = x.RecognitionResults.SelectMany(p => p.Lines.Select(l => l.Text)).ToArray();
            var yLines = y.RecognitionResults.SelectMany(p => p.Lines.Select(l => l.Text)).ToArray();
            if (xLines.Length != yLines.Length) { return false; }
            for (int i = 0; i < xLines.Length; i++) {
                if (xLines[i] != yLines[i]) {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(MsReadParseResult obj)
        {
            return obj.GetHashCode();
        }
    }
}


