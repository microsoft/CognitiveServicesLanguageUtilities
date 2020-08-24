using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.CommandsLayer.ConfigCommand.Show
{
    [Command("parser", Description = "shows configs for all parsers")]
    [Subcommand(
        typeof(ConfigShowMsReadCommand))]
    class ConfigShowParserCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowParserConfigs();
            }
            return 1;
        }
    }
}
