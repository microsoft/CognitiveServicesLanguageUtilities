using Autofac;
using CliTool.Configs;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config.Set
{
    [Command("blob")]
    class ConfigSetBlobCommand
    {
        [Option("--connection-string <CONNECTION_STRING>")]
        public string ConnectionString { get; }

        [Option("--source-container <CONTAINER_NAME>")]
        public string SourceContainer { get; }

        [Option("--destination-container <CONTAINER_NAME>")]
        public string DestinationContainer { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetBlobStorageConfigs(ConnectionString, SourceContainer, DestinationContainer);
            }

            return 1;
        }
    }
}
