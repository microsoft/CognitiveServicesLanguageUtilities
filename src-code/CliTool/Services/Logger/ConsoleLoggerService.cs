using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Services.Logger
{
    class ConsoleLoggerService : ILoggerService
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
