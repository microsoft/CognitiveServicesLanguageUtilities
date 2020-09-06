using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("chunker", Description = "sets configs for chunker")]
    public class ConfigSetChunkerCommand
    {
        [Required]
        [Range(Constants.MinAllowedCharLimit, Constants.CustomTextPredictionMaxCharLimit)]
        [Option(CommandOptionTemplate.ChunkerCharLimit, Description = "character limit for chunk")]
        public int CharLimit { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                await controller.SetChunkerConfigsAsync(CharLimit);
            }
        }
    }
}
