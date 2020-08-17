using CliTool.Exceptions;
using System;

namespace CliTool.Services.Logger
{
    interface ILoggerService
    {
        public void Log(string message);

        public void LogCustomError(CliException e);
    }
}
