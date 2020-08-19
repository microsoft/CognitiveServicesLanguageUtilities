using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config.Set
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
