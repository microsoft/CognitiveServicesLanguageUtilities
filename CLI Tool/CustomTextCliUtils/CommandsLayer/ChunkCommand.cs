using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace Microsoft.CustomTextCliUtils.CommandsLayer
{
    [Command("chunk", Description = "chunk text file")]
    public class ChunkCommand
    {
        [Required]
        [Option("--source <local/blob>", Description = "[required] indicates source storage type")]
        public StorageType Source { get; }
        [Required]
        [Option("--destination <local/blob>", Description = "[required] indicates destination storage type")]
        public StorageType Destination { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildChunkerCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ChunkerServiceController>();
                controller.ChunkTextAsync(Source, Destination).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            return 0;
        }
    }
}
