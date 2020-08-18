
using Autofac;
using CliTool.Configs;
using CliTool.Configs.Models.Enums;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CliTool.Commands
{
    [Command("parse")]
    class ParseCommand
    {
        [Required]
        [Option("--parser <msread/tika>")]
        public ParserType Parser { get; }
        [Required]
        [Option("--source <local/blob>")]
        public StorageType Source { get; }
        [Required]
        [Option("--destination <local/blob>")]
        public StorageType Destination { get; }
        [Option("--chunk-type <page/char>")]
        public ChunkType ChunkType { get; }

        private async Task<int> OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildParseCommandDependencies(Parser, Source, Destination);

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserServiceController>();
                // controller.SetStorageServices(Source, Destination);
                await controller.ExtractText();
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
