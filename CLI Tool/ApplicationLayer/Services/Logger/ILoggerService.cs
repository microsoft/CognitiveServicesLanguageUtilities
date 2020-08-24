using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using System;
using System.Collections.Generic;

namespace CustomTextCliUtils.ApplicationLayer.Services.Logger
{
    interface ILoggerService
    {
        public void Log(string message);

        public void LogError(Exception e);

        public void LogOperation(OperationType operationType, string message);

        void LogParsingResult(List<string> convertedFiles, List<string> failedFiles);
    }
}
