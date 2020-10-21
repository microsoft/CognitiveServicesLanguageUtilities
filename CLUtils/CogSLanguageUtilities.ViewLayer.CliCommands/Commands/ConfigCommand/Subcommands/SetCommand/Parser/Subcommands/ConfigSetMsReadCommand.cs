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
    [Command("msread", Description = "sets configs for msread parser")]
    public class ConfigSetMsReadCommand
    {
        [Option(CommandOptionTemplate.MSReadAzureResourceKey, Description = "azure congnitive services key")]
        public string CognitiveServicesKey { get; }

        [Option(CommandOptionTemplate.MSReadAzureResourceEndpoint, Description = "endpoint url for azure congnitive services")]
        public string EndpointUrl { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigsController>();
                await controller.SetMsReadConfigsAsync(CognitiveServicesKey, EndpointUrl);
            }
        }
    }
}
