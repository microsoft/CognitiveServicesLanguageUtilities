using System;

namespace CognitiveSearchIntegration.Common.Logging
{
    public interface ILoggerService
    {
        public void LogOperation(OperationType operationType, string message = "");
        public void LogSuccessMessage(string message);
        public void LogUnhandledError(Exception exception);
    }
}
