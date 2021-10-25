// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;

namespace Microsoft.IAPUtilities.Definitions.Exceptions
{
    public class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        { }
    }
}
