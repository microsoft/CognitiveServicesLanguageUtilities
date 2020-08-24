using CustomTextCliUtils.AppController.Models.ServiceModels.Parser;
using CustomTextCliUtils.AppController.Services.Chunker;
using System.IO;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.Services.Parser
{
    interface IParserService
    {
        public Task<ParseResult> ParseFile(Stream file);

        public void ValidateFileType(string fileType);
    }
}
