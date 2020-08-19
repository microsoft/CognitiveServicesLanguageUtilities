using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config.Show
{
    [Command("storage")]
    [Subcommand(
        typeof(ConfigShowLocalCommand),
        typeof(ConfigShowBlobCommand))]
    class ConfigShowStorageCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowStorageConfigs();
            }
            return 1;
        }
    }
}
