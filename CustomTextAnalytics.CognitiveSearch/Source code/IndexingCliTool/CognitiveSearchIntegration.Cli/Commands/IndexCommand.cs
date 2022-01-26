using CognitiveSearchIntegration.Runners;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Cli.Commands
{
    [Command("index", Description = "")]
    public class IndexCommand
    {
        [Option("--index-name <absolute_path>", Description = "name of index to be created")]
        [Required]
        public string IndexName { get; }
        [Option("--configs <absolute_path>", Description = "path of configs file")]
        public string Configs { get; }

        public async Task OnExecute(CommandLineApplication app)
        {
            await IndexingOperationRunner.RunOperation(IndexName, Configs);
        }
    }
}
