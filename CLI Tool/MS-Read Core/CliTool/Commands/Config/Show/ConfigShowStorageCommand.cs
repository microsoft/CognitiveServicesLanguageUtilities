using Autofac;
using CliTool.Configs;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config.Show
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
