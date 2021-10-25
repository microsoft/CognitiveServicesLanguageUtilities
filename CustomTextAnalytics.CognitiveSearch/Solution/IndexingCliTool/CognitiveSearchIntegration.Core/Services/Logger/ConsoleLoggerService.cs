using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Enums.Logger;
using System;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.Logger
{
    public class ConsoleLoggerService : ILoggerService
    {
        private static readonly object _lock = new object();

        public void LogOperation(OperationType operationType, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(operationType.ToString() + ":\t");
                Console.ResetColor();
                Console.WriteLine(message);
            }
        }

        public void LogSuccessMessage(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Successful: ");
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public void LogUnhandledError(Exception exception)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: ");
                Console.ResetColor();
                Console.WriteLine(exception.Message);
            }
        }
    }
}
