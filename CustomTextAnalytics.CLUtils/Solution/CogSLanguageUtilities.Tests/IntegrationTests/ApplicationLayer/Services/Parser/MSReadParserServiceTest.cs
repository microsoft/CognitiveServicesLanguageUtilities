// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser;
using Microsoft.CogSLanguageUtilities.Tests.Configs;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.IntegrationTests.Services.Parser
{
    public class MSReadParserServiceTest
    {
        // Test Connection to MSRead Cognitive Service
        // ######################################################################
        public static TheoryData TestConnectionData()
        {
            // test 1 data
            string test1CognitiveServicesKey = Secrets.MSReadCongnitiveServicesKey;
            string test1CognitiveServicesEndpoint = Secrets.MSReadCognitiveServicesEndPoint;
            // test 2 data - wrong service key
            string test2Message = "";
            string test2CognitiveServicesKey = "2e10bb66ed3e7685bd3ca1b3abcd8f6b";
            string test2CognitiveServicesEndpoint = Secrets.MSReadCognitiveServicesEndPoint;
            // test 3 data - wrong endpoint
            string test3Message = "";
            string test3CognitiveServicesKey = Secrets.MSReadCongnitiveServicesKey;
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
                    new MsReadConnectionException(test2Message)
                },
                {
                    // wrong endpoint
                    test3CognitiveServicesKey,
                    test3CognitiveServicesEndpoint,
                    new MsReadConnectionException(test3Message)
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
            else
            {
                Assert.Throws(expectedException.GetType(), () =>
                {
                    new MSReadParserService(cognitiveServiceEndPoint, congnitiveServiceKey);
                });
            }
        }


        // Test Parser Mapping
        // ######################################################################
        public static TheoryData TestParsingData()
        {
            // test 1 data
            var inputDocument = File.OpenRead(@"TestData\Parser\MSRead\test1 - inputDocument.pdf");  // read input document
            var parser = new MSReadParserService(Secrets.MSReadCognitiveServicesEndPoint, Secrets.MSReadCongnitiveServicesKey);

            return new TheoryData<Stream, MSReadParserService, CliException>
            {
                {
                    inputDocument,
                    parser,
                    null
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestParsingData))]
        public void TestParsing(Stream inputDocument, MSReadParserService parser, CliException expectedException)
        {
            /* TEST NOTES
             * *************
             * we only care about parsing result
             * i.e. parser results maps to our object correctly
             * 
             * we don't care about the actual values in the object
             * because the service provider (in this case msread team)
             * may optimize their engine
             * rendering the values in our "ExpectedResult" object in correct
             * */
            if (expectedException == null)
            {
                var actualResult = parser.ParseFileInternal(inputDocument).ConfigureAwait(false).GetAwaiter().GetResult();
                // validate object values aren't null
                actualResult.AnalyzeResult.ReadResults.ToList().ForEach(page =>
                {
                    // check all properties
                    Assert.NotNull(page.Lines);
                    // check line objects
                    page.Lines.ToList().ForEach(line =>
                    {
                        Assert.NotNull(line.BoundingBox);
                        Assert.NotNull(line.Text);
                        Assert.NotNull(line.Words);
                    });
                });
            }
            else
            {
                Assert.Throws(expectedException.GetType(), () =>
                {
                    parser.ParseFile(inputDocument).ConfigureAwait(false).GetAwaiter().GetResult();
                });
            }
        }
    }
}


