using Autofac;
using CliTool.Configs;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace CliTool.CommandControllers
{
    class ParseCommandController : ICommandController
    {
        [Option("--parser <msread/tika>")]
        public string Parser { get; }
        [Option("--source <local/blob>")]
        public string Source { get; }
        [Option("--destination <local/blob>")]
        public string Destination { get; }
        [Option("--chunk-type <page/char>")]
        public string ChunkType { get; }


        public async void Execute(string[] args)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ParserServiceController>();
                await controller.ExtractText();
            }
        }

        public void ValidateArgs()
        {
            throw new NotImplementedException();
        }

        public void ExtractArgs(string[] args) {
            // input: parse --parser {MSREAD/TIKA} --source {BLOB/LOCAL} --destination {BLOB/LOCAL} --chunking-type {PAGE/CHAR}
            var i = 1;
            while (i < args.Length) { 
                
            }
        }
    }
}
