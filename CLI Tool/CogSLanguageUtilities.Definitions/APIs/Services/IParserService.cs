using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IParserService
    {
        public Task<ParseResult> ParseFile(Stream file);

        public void ValidateFileType(string fileType);
    }
}
