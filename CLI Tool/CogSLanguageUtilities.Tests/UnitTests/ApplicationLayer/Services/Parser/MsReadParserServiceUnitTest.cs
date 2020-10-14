using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
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

        public class DocumentTreeComparer : IEqualityComparer<DocumentTree>
        {
            public bool Equals(DocumentTree x, DocumentTree y)
            {
                var list = x.DocumentSegments.Zip(y.DocumentSegments, (e1, e2) => new { e1, e2 });
                foreach (var entry in list)
                {
                    if (!EqualsInternal(entry.e1, entry.e2))
                    {
                        return false;
                    }
                }
                return true;
            }

            private bool EqualsInternal(DocumentSegment segment1, DocumentSegment segment2)
            {
                if (segment1.RootElement.PageNumber != segment2.RootElement.PageNumber || segment1.RootElement.Text != segment2.RootElement.Text)
                {
                    return false;
                }
                if (segment1.Children != null)
                {
                    if (segment2.Children == null || segment1.Children.Count != segment2.Children.Count)
                    {
                        return false;
                    }
                    var list = segment1.Children.Zip(segment2.Children, (e1, e2) => new { e1, e2 });
                    foreach (var entry in list)
                    {
                        if (!EqualsInternal(entry.e1, entry.e2))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public int GetHashCode(DocumentTree obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
