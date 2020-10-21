// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Prediction
{
    public class UnauthorizedRequestException : CliException
    {
        public UnauthorizedRequestException(string url, string key)
            : base(ConstructMessage(url, key))
        { }

        public static string ConstructMessage(string url, string key)
        {
            return $"Unauthorized Request to {url} \nusing key {key}";
        }
    }
}
