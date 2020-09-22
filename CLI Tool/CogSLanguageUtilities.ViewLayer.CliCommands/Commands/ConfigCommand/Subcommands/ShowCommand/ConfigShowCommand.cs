using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CustomTextCliUtils.Configs;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("show", Description = "shows app configs")]
    [Subcommand(
        typeof(ConfigShowParserCommand),
        typeof(ConfigShowStorageCommand),
        typeof(ConfigShowChunkerCommand),
        typeof(ConfigShowCustomTextCommand),
        typeof(ConfigShowTextAnalyticsCommand))]
    public class ConfigShowCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigsController>();
                controller.ShowAllConfigs();
            }
        }
    }
}
