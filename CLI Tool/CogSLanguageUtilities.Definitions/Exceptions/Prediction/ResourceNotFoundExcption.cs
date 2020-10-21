// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Prediction
{
    public class ResourceNotFoundExcption : CliException
    {
        public ResourceNotFoundExcption(string message)
            : base(message)
        { }
    }
}
