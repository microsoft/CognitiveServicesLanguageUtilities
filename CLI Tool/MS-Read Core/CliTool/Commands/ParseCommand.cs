
using Autofac;
using CliTool.Configs;
using CliTool.Configs.Models.Enums;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace CliTool.Commands
{
    [Command("parse")]
    class ParseCommand
    {
        [Option("--parser <msread/tika>")]
        public ParserType Parser { get; }
        [Option("--source <local/blob>")]
        public StorageType Source { get; }
        [Option("--destination <local/blob>")]
        public StorageType Destination { get; }
        [Option("--chunk-type <page/char>")]
        public ChunkType ChunkType { get; }

        protected async Task<int> OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserServiceController>();
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
