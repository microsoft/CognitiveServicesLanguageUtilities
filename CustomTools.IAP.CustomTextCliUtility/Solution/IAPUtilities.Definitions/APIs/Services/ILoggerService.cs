// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.Models.Enums.Logger;
using System;
using System.Collections.Generic;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface ILoggerService
    {
        public void Log(string message);

        public void LogError(Exception e);

        public void LogOperation(OperationType operationType);

        public void LogOperation(OperationType operationType, string message);

        public void LogParsingResult(IEnumerable<string> convertedFiles, IDictionary<string, string> failedFiles);
    }
}
