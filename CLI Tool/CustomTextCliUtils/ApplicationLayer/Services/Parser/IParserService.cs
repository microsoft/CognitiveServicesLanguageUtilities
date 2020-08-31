using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser
{
    public interface IParserService
    {
        public Task<ParseResult> ParseFile(Stream file);

        public void ValidateFileType(string fileType);
    }
}
