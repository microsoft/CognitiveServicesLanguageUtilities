// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CustomTextCliUtils.Configs;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("customtext", Description = "shows configs for Custom Text")]
    [Subcommand(
        typeof(ConfigShowCustomTextAuthoringCommand),
        typeof(ConfigShowCustomTextPredictionCommand))]
    public class ConfigShowCustomTextCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigsController>();
                controller.ShowCustomTextConfigs();
            }
        }
    }
}
