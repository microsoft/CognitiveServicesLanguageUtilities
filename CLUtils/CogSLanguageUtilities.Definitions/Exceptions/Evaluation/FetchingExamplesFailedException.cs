// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Evaluation
{
    public class FetchingExamplesFailedException : CliException
    {
        public FetchingExamplesFailedException(string statusCode)
            : base(ConstructMessage(statusCode))
        { }

        public static string ConstructMessage(string statusCode)
        {
            return $"Failed to fetch labeled examples from custom text application with status code: {statusCode}";
        }
    }
}
