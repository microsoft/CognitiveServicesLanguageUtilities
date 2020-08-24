using Autofac;
using CustomTextCliUtils.CommandsLayer.ConfigCommand.Show;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.CommandsLayer.ConfigCommand
{
    [Command("show", Description = "shows app configs")]
    [Subcommand(
        typeof(ConfigShowParserCommand),
        typeof(ConfigShowStorageCommand),
        typeof(ConfigShowChunkerCommand),
        typeof(ConfigShowPredictionCommand))]
    class ConfigShowCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowAllConfigs();
            }
            return 1;
        }
    }
}
