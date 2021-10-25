// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.Configs.Consts;

namespace Microsoft.IAPUtilities.Definitions.Exceptions.Configs
{
    public class MissingConfigsException : CliException
    {
        public MissingConfigsException()
            : base(ConstructMessage())
        { }

        public static string ConstructMessage()
        {
            return $"Please add the required configs using the command\n{Constants.ToolName} config set";
        }
    }
}
