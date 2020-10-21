// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;

namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser
{
    public class MsReadConnectionException : CliException
    {
        public MsReadConnectionException(string message)
            : base(ConstructMessage(message))
        { }

        private static string ConstructMessage(string message)
        {
            return $"Connection to Azure Cognitive Services failed with message: \"{message}\" \nCheck MsRead configs" +
                $"{ConfigKeys.MSReadAzureResourceKey} and {ConfigKeys.MSReadAzureResourceEndpoint}";
        }
    }
}
