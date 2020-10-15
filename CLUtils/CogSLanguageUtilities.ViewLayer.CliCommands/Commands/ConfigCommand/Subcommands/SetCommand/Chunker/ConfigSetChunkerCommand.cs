// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using Microsoft.CustomTextCliUtils.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("chunker", Description = "sets configs for chunker")]
    public class ConfigSetChunkerCommand
    {
        [Range(Definitions.Configs.Consts.Constants.MinAllowedCharLimit, Definitions.Configs.Consts.Constants.CustomTextPredictionMaxCharLimit)]
        [Option(CommandOptionTemplate.ChunkerCharLimit, Description = "character limit for chunk")]
        public int CharLimit { get; }

        [Range((int)Definitions.Configs.Consts.Constants.minAllowdChunkSectionLevel, (int)Definitions.Configs.Consts.Constants.maxAllowdChunkSectionLevel)]
        [Option(CommandOptionTemplate.ChunkerSectionLevel, Description = "logical section level (i.e. chunk document by title, h1, h2, h3)")]
        public ElementType ChunkSectionLevel { get; } = ElementType.Other;

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigsController>();
                await controller.SetChunkerConfigsAsync(CharLimit, ChunkSectionLevel);
            }
        }
    }
}
