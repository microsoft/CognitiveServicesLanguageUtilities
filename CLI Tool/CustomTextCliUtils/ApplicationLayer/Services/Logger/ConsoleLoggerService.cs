using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger
{
    public class ConsoleLoggerService : ILoggerService
    {
        static readonly object _lock = new object();

        public void Log(string message)
        {
            lock (_lock)
            {
                Console.WriteLine(message);
            }
        }

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

        public void LogParsingResult(List<string> convertedFiles, Dictionary<string, string> failedFiles)
        {
            var totalFilesCount = convertedFiles.Count + failedFiles.Count;
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Total Files: {0},\tSuccessfully Converted: {1},\tFailed: {2}",
                    totalFilesCount, convertedFiles.Count, failedFiles.Count);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Converted:");
                convertedFiles.ForEach(f => Console.WriteLine("\t{0}", f));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed:");
                failedFiles.ToList().ForEach(kv => Console.WriteLine("\t{0}\t=> {1}", kv.Key, kv.Value));
                Console.ResetColor();
            }
        }

        public void LogError(Exception e)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: ");
                Console.ResetColor();
                Console.WriteLine(e.Message);
            }
        }
    }
}
