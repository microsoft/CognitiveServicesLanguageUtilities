using Autofac;
using CustomTextCliUtils.Commands.Config.Show;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config
{
    [Command("show", Description = "shows app configs")]
    [Subcommand(
        typeof(ConfigShowParser),
        typeof(ConfigShowStorageCommand))]
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
