using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CognitiveSearchIntegration.Core.Orchestrators;
using Microsoft.CognitiveSearchIntegration.Enums.Prediction;
using Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Commands
{
    [Command("index", Description = "")]
    public class IndexCommand
    {
        [Option("--schema <absolute_path>", Description = "absolute path to schema file")]
        [Required]
        public string Schema { get; }

        [Option("--cognitive-service <cognitive_service>", Description = "")]
        public CognitiveServiceType CognitiveSerice { get; } = CognitiveServiceType.CustomText;

        [Option("--index-name <absolute_path>", Description = "name of index to be created")]
        [Required]
        public string IndexName { get; }

        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DIController.BuildIndexCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IndexingOrchestrator>();
                await controller.IndexCustomText(Schema, IndexName);
            }
        }
    }
}
