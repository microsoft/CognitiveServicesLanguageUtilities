// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Prediction
{
    public class BadRequestException : CliException
    {
        public BadRequestException(string message)
            : base(message)
        { }
    }
}
