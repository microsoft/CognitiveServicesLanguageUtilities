// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Prediction;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.CommandHelp;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Exceptions;
using Microsoft.CustomTextCliUtils.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.PredictCommand
{
    [Command("predict")]
    [SuppressDefaultHelpOption]
    public class PredictCommand
    {
        [Option("--cognitive-service <customtext/textanalytics/both>")]
        public CognitiveServiceType CognitiveService { get; }
        [Option("--parser <msread/tika>")]
        public ParserType Parser { get; }
        [Option("--source <local/blob>")]
        public StorageType Source { get; }
        [Option("--destination <local/blob>")]
        public StorageType Destination { get; }
        [Option("--chunk-type <page/char>")]
        public ChunkMethod ChunkType { get; } = ChunkMethod.NoChunking;
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
            var container = DependencyInjectionController.BuildPredictCommandDependencies(Parser);

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<PredictionController>();
                await controller.Predict(Source, Destination, ChunkType, CognitiveService);
            }
        }

        private bool CheckHelpRequested()
        {
            // in case no arguments passed or --help specified
            // but according to Demorgan laws, --help won't matter (noOptions && noHelp) || (noOptions && help)
            return
                CognitiveService == CognitiveServiceType.NotSpecified
                && Parser == ParserType.NotSpecified
                && Source == StorageType.NotSpecified
                && Destination == StorageType.NotSpecified
                && ChunkType == ChunkMethod.NoChunking;
        }

        private void ValidateArguments()
        {
            if (Help == true)
            {
                throw new InvalidArgumentException("--help");
            }

            if (CognitiveService == CognitiveServiceType.NotSpecified)
            {
                throw new RequiredArgumentException("--cognitive-service");
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
