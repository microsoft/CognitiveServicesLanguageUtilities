using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.Services.Parser
{
    interface IParserService
    {
        public Task<string> ExtractText(FileStream file);
    }
}
