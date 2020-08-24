using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.Models.Enums;
using CustomTextCliUtils.AppController.ServiceControllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CustomTextCliUtils.Commands
{
    [Command("predict", Description = "uses custom text to predict intents and entities in given document")]
    class PredictCommand
    {
        [Required]
        [Option("--parser <msread/tika>", Description = "[required] indicates which parsing tool to use")]
        public ParserType Parser { get; }
        [Required]
        [Option("--source <local/blob>", Description = "[required] indicates source storage type")]
        public StorageType Source { get; }
        [Required]
        [Option("--destination <local/blob>", Description = "[required] indicates destination storage type")]
        public StorageType Destination { get; }
        [Required]
        [Option("--file-name <FILE_NAME>", Description = "[required] target file to use for prediction")]
        public string FileName { get; }
        [Option("--chunk-type <page/char>", Description = "[optional] indicates chunking type. if not set, no chunking will be used")]
        public ChunkMethod ChunkType { get; } = ChunkMethod.NoChunking;

        private async Task<int> OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildPredictCommandDependencies(Parser);

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<PredictionServiceController>();
                // controller.SetStorageServices(Source, Destination);
                await controller.Predict(Source, Destination, FileName, ChunkType);
            }

            return 0;
        }
    }
}
