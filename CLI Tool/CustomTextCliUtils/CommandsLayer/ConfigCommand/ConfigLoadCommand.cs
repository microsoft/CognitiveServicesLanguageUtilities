using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand
{
    [Command("load", Description = "loads app configs from file")]
    public class ConfigLoadCommand
    {
        [Option("--path <absolute_path>", Description = "absolute path to configs file")]
        public string configsFilePath { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                await controller.LoadConfigsFromFile(configsFilePath);
            }
        }
    }
}
