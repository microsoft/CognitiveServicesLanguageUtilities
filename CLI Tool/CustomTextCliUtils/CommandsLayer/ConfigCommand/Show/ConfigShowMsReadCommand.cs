using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.CommandsLayer.ConfigCommand.Show
{
    [Command("msread", Description = "shows configs for msread parser")]
    class ConfigShowMsReadCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowParserMsReadConfigs();
            }
            return 1;
        }
    }
}
