using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.UnitTests.ApplicationLayer.Services.Parser
{
    public class PlainTextParserServiceUnitTest
    {
        private PlainTextParserService _parser;

        public PlainTextParserServiceUnitTest()
        {
            _parser = new PlainTextParserService();
        }

        public static TheoryData ParseTextTestData()
        {
            var inputDocument = File.OpenRead(@"TestData\Parser\Text\test1.txt");  // read input document
            var expected = JsonConvert.DeserializeObject<DocumentTree>(File.ReadAllText(@"TestData\Parser\Text\expected.json"));
            return new TheoryData<Stream, DocumentTree>
            {
                {
                    inputDocument,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(ParseTextTestData))]
        public async Task ParseTextTestAsync(Stream inputFile, DocumentTree expected)
        {
            var actual = await _parser.ParseFile(inputFile);
            Assert.Equal(actual.RootSegment.Children.First().RootElement.Text, expected.RootSegment.Children.First().RootElement.Text);
            Assert.Equal(actual.RootSegment.Children.First().RootElement.Type, expected.RootSegment.Children.First().RootElement.Type);
        }
    }
}
