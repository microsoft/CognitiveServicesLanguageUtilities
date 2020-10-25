// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.CommandHelp;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Exceptions;
using Microsoft.CustomTextCliUtils.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.UtilitiesCommand
{
    [Command("chunk")]
    [SuppressDefaultHelpOption]
    public class ChunkCommand
    {
        [Option("--source <local/blob>")]
        public StorageType Source { get; }
        [Option("--destination <local/blob>")]
        public StorageType Destination { get; }
        [Option("-h|--help")]
        public bool Help { get; } = false;

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // show help if requested
            if (CheckHelpRequested())
            {
                app.HelpTextGenerator = new CustomHelpTextGenerator();
                app.ShowHelp();
                return;
            }

            // validate required args
            ValidateArguments();

            // build dependencies
            var container = DependencyInjectionController.BuildChunkerCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ChunkerController>();
                await controller.ChunkTextAsync(Source, Destination);
            }
        }

        private bool CheckHelpRequested()
        {
            // in case no arguments passed or --help specified
            // but according to Demorgan laws, --help won't matter (noOptions && noHelp) || (noOptions && help)
            return
                Source == StorageType.NotSpecified
                && Destination == StorageType.NotSpecified;
        }

        private void ValidateArguments()
        {
            if (Help == true)
            {
                throw new InvalidArgumentException("--help");
            }

            if (Source == StorageType.NotSpecified)
            {
                throw new RequiredArgumentException("--source");
            }

            if (Destination == StorageType.NotSpecified)
            {
                throw new RequiredArgumentException("--destination");
            }
        }
    }
}
