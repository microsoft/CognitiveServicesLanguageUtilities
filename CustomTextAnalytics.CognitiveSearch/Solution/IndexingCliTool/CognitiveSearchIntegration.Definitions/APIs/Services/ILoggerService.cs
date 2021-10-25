using Microsoft.CognitiveSearchIntegration.Definitions.Enums.Logger;
using System;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services
{
    public interface ILoggerService
    {
        public void LogOperation(OperationType operationType, string message = "");
        public void LogSuccessMessage(string message);
        public void LogUnhandledError(Exception exception);
    }
}
