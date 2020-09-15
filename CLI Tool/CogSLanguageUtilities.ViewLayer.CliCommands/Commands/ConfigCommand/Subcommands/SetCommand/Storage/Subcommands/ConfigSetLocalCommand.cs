using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
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
                var controller = scope.Resolve<ConfigsController>();
                await controller.SetLocalStorageConfigsAsync(SourceDirectory, DestinationDirectory);
            }
        }
    }
}
