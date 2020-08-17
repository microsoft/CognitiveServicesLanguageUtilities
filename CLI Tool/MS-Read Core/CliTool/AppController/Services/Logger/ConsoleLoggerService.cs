using CliTool.Exceptions;
using System;

namespace CliTool.Services.Logger
{
    class ConsoleLoggerService : ILoggerService
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogCustomError(CliException e)
        {
            Console.WriteLine("Error: " + e.CustomMessage);
        }
    }
}
