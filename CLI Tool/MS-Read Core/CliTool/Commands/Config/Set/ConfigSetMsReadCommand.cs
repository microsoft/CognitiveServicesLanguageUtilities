using Autofac;
using CliTool.Configs;
using CliTool.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config.Set
{
    [Command("msread")]
    class ConfigSetMsReadCommand
    {
        [Option("--cognitive-services-key <>")]
        public string CognitiveServicesKey { get; }

        [Option("--endpoint-url <ENDPOINT_URL>")]
        public string EndpointUrl { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetMsReadConfigs(CognitiveServicesKey, EndpointUrl);
            }

            return 1;
        }
    }
}
