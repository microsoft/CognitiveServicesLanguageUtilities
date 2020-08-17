using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.ServiceControllers
{
    interface IParserServiceController
    {
        public Task ExtractText();
    }
}
