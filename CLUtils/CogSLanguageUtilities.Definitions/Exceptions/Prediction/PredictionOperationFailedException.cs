// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Prediction
{
    public class PredictionOperationFailedException : CliException
    {
        public PredictionOperationFailedException(string operationId)
            : base(ConstructMessage(operationId))
        { }

        public PredictionOperationFailedException(string operationId, string errorDetails)
            : base(ConstructMessage(operationId, errorDetails))
        { }

        public static string ConstructMessage(string operationId)
        {
            return $"Prediction operation with ID {operationId} failed";
        }

        public static string ConstructMessage(string operationId, string errorDetails)
        {
            return $"Prediction operation with ID {operationId} failed with message '{errorDetails}'";
        }
    }
}
