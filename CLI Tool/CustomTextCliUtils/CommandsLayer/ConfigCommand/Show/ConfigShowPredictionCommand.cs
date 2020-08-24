using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.CommandsLayer.ConfigCommand.Show
{
    [Command("prediction", Description = "shows configs for all prediction")]
    class ConfigShowPredictionCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowPredictionConfigs();
            }
            return 1;
        }
    }
}
