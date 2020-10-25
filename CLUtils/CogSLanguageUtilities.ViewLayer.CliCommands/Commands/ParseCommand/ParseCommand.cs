// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.CommandHelp;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Exceptions;
using Microsoft.CustomTextCliUtils.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.UtilitiesCommand
{
    [Command("parse")]
    [SuppressDefaultHelpOption]
    public class ParseCommand
    {
        [Option("--parser <msread/tika>")]
        public ParserType Parser { get; } = ParserType.NotSpecified;
        [Option("--source <local/blob>")]
        public StorageType Source { get; } = StorageType.NotSpecified;
        [Option("--destination <local/blob>")]
        public StorageType Destination { get; } = StorageType.NotSpecified;
        [Option("--chunk-type <page/char>")]
        public ChunkMethod ChunkType { get; } = ChunkMethod.NotSpecified;
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
            var container = DependencyInjectionController.BuildParseCommandDependencies(Parser);

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserController>();
                await controller.ExtractText(Source, Destination, ChunkType);
            }
        }

        private bool CheckHelpRequested()
        {
            // in case no arguments passed or --help specified
            // but according to Demorgan laws, --help won't matter (noOptions && noHelp) || (noOptions && help)
            return
                Parser == ParserType.NotSpecified
                && Source == StorageType.NotSpecified
                && Destination == StorageType.NotSpecified
                && ChunkType == ChunkMethod.NotSpecified;
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
