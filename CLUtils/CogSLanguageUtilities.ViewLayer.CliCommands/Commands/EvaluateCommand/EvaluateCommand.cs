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

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.EvaluateCommand
{
    [Command("evaluate")]
    [SuppressDefaultHelpOption]
    public class EvaluateCommand
    {
        [Option("--source <local/blob>")]
        public StorageType Source { get; } = StorageType.Blob;
        [Option("--destination <local/blob>")]
        public StorageType Destination { get; }
        [Option("-h|--help")]
        public bool Help { get; } = false;

        private async Task OnExecute(CommandLineApplication app)
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
            var container = DependencyInjectionController.BuildEvaluateCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<BatchTestingController>();
                await controller.EvaluateCustomTextAppAsync(Source, Destination);
            }
        }

        private bool CheckHelpRequested()
        {
            // in case no arguments passed or --help specified
            // but according to Demorgan laws, --help won't matter (noOptions && noHelp) || (noOptions && help)
            return Destination == StorageType.NotSpecified;
        }

        private void ValidateArguments()
        {
            if (Help == true)
            {
                throw new InvalidArgumentException("--help");
            }

            if (Destination == StorageType.NotSpecified)
            {
                throw new RequiredArgumentException("--destination");
            }
        }
    }
}

