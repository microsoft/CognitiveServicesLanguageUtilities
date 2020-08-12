using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Services.Logger
{
    interface ILoggerService
    {
        public void Log(string message);
    }
}
