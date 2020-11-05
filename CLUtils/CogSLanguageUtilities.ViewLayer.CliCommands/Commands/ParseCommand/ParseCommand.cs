// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.UtilitiesCommand
{
    [Command("parse", Description = "extract text from all documents in source storage and stores result in destination storage")]
    public class ParseCommand
    {
        [Required]
        [Option("--source <local/blob>", Description = "[required] indicates source storage type")]
        public StorageType Source { get; }
        [Required]
        [Option("--destination <local/blob>", Description = "[required] indicates destination storage type")]
        public StorageType Destination { get; }
        [Option("--chunk-type <page/char>", Description = "[optional] indicates chunking type. if not set, no chunking will be used")]
        public ChunkMethod ChunkType { get; } = ChunkMethod.NoChunking;

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildParseCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserController>();
                await controller.ExtractText(Source, Destination, ChunkType);
            }
        }
    }
}
