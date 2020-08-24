using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using System.IO;
using System.Threading.Tasks;

namespace CustomTextCliUtils.ApplicationLayer.Services.Parser
{
    interface IParserService
    {
        public Task<ParseResult> ParseFile(Stream file);

        public void ValidateFileType(string fileType);
    }
}
