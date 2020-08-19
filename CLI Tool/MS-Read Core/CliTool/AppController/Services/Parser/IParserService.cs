using System.IO;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.Services.Parser
{
    interface IParserService
    {
        public Task<string> ExtractText(Stream file, string fileName);

        public void ValidateFileType(string fileType);
    }
}
