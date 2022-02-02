using FileFormatConverter.Runner.DataStructures;
using System;

namespace FileFormatConverter.Runner.Services
{
    public class LoggingService
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

        public void LogSuccess(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Successful: ");
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public void LogError(Exception exception)
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
