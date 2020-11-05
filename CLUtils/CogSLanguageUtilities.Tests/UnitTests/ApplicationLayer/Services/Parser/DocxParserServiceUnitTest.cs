using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using Microsoft.CogSLanguageUtilities.Tests.TestUtils;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.UnitTests.ApplicationLayer.Services.Parser
{
    public class DocxParserServiceUnitTest
    {
        private DocxParserService _parser;

        public DocxParserServiceUnitTest()
        {
            _parser = new DocxParserService();
        }

        public static TheoryData ParseDocxTestData()
        {
            var inputFile = File.OpenRead(@"TestData\Parser\Docx\test1.docx");
            var expectedString = File.ReadAllText(@"TestData\Parser\Docx\test1-expected.json");
            var expected = JsonConvert.DeserializeObject<DocumentTree>(expectedString);
            return new TheoryData<Stream, DocumentTree>
            {
                {
                    inputFile,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(ParseDocxTestData))]
        public async Task ParseDocxTest(Stream file, DocumentTree expected)
        {
            var actual = await _parser.ParseFile(file);
            Assert.Equal(expected, actual, new DocumentTreeComparer());
        }
    }
}
