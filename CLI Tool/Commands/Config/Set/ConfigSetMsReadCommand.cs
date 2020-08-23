using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers;
using McMaster.Extensions.CommandLineUtils;
using CustomTextCliUtils.Configs.Consts;

namespace CustomTextCliUtils.Commands.Config.Set
{
    [Command("msread", Description = "sets configs for msread parser")]
    class ConfigSetMsReadCommand
    {
        [Option(CommandOptionTemplate.MSReadCognitiveServicesKey, Description = "azure congnitive services key")]
        public string CognitiveServicesKey { get; }

        [Option(CommandOptionTemplate.MSReadCognitiveServicesEndpoint, Description = "endpoint url for azure congnitive services")]
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
