using System.IO;
using System.Threading.Tasks;

namespace CliTool.Services.Parser
{
    interface IParserService
    {
        public Task<string> ExtractText(Stream file);

        public void ValidateFileType(string fileType);
    }
}
