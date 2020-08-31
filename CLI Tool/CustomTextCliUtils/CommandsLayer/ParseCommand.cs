using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace Microsoft.CustomTextCliUtils.CommandsLayer
{
    [Command("parse", Description = "extract text from all documents in source storage and stores result in destination storage")]
    class ParseCommand
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
        [Option("--chunk-type <page/char>", Description = "[optional] indicates chunking type. if not set, no chunking will be used")]
        public ChunkMethod ChunkType { get; } = ChunkMethod.NoChunking;

        private async Task<int> OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildParseCommandDependencies(Parser);

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserServiceController>();
                // controller.SetStorageServices(Source, Destination);
                await controller.ExtractText(Source, Destination, ChunkType);
            }

            return 0;
        }
    }
}
