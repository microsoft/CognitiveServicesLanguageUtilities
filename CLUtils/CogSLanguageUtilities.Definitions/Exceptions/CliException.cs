// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using System;

namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions
{
    public class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        { }
    }
}
