// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("prediction", Description = "sets configs for Custom Text Prediction")]
    public class ConfigSetCustomTextPredictionCommand
    {
        [Option(CommandOptionTemplate.CustomTextAzureResourceKey, Description = "custom text app prediction resource key")]
        public string CustomTextKey { get; }
        [Option(CommandOptionTemplate.CustomTextAzureResourceEndpoint, Description = "custom text app prediction resource endpoint url")]
        public string EndpointUrl { get; }
        [Option(CommandOptionTemplate.CustomTextAppId, Description = "custom text app id")]
        public string AppId { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigsController>();
                await controller.SetCustomTextPredictionConfigsAsync(CustomTextKey, EndpointUrl, AppId);
            }
        }
    }
}
