using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Show
{
    [Command("msread", Description = "shows configs for msread parser")]
    public class ConfigShowMsReadCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowParserMsReadConfigs();
            }
        }
    }
}
