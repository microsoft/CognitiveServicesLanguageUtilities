using Autofac;
using Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Show;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand
{
    [Command("show", Description = "shows app configs")]
    [Subcommand(
        typeof(ConfigShowParserCommand),
        typeof(ConfigShowStorageCommand),
        typeof(ConfigShowChunkerCommand),
        typeof(ConfigShowPredictionCommand),
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
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowAllConfigs();
            }
        }
    }
}
