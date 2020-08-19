using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.Models.Enums;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CustomTextCliUtils.Commands
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
            var container = DependencyInjectionController.BuildParseCommandDependencies(Parser, Source, Destination);

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserServiceController>();
                // controller.SetStorageServices(Source, Destination);
                await controller.ExtractText(Source, Destination, ChunkType);
            }

            return 0;
        }

        public void PrintCommandData()
        {
            Console.WriteLine("Parser : " + Parser);
            Console.WriteLine("Source : " + Source);
            Console.WriteLine("Desitnation : " + Destination);
            Console.WriteLine("ChunkType : " + ChunkType);
        }
    }
}
