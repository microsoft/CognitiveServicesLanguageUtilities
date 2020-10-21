// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("customtext", Description = "sets configs for Custom Text")]
    [Subcommand(
        typeof(ConfigSetCustomTextAuthoringCommand),
        typeof(ConfigSetCustomTextPredictionCommand))]
    public class ConfigSetCustomTextCommand
    {
        private void OnExecuteAsync(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
