using Autofac;
using CliTool.Commands.Config.ConfigShow;
using CliTool.Configs;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config
{
    [Command("show")]
    [Subcommand(
        typeof(ConfigShowMsReadCommand),
        typeof(ConfigShowLocalCommand),
        typeof(ConfigShowBlobCommand))]
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
