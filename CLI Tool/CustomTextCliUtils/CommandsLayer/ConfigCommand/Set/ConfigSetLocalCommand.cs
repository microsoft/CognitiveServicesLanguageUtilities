using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("local", Description = "sets configs for local storage")]
    public class ConfigSetLocalCommand
    {
        [Option(CommandOptionTemplate.LocalStorageSourceDir, Description = "absolute path for source directory")]
        public string SourceDirectory { get; }

        [Option(CommandOptionTemplate.LocalStorageDestinationDir, Description = "absolute path for destination directory")]
        public string DestinationDirectory { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                await controller.SetLocalStorageConfigsAsync(SourceDirectory, DestinationDirectory);
            }
        }
    }
}
