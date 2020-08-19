using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config.Set
{
    [Command("blob", Description = "sets configs for blob storage")]
    class ConfigSetBlobCommand
    {
        [Option("--connection-string <CONNECTION_STRING>", Description = "azure storage account connection string")]
        public string ConnectionString { get; }

        [Option("--source-container <CONTAINER_NAME>", Description = "name of source container")]
        public string SourceContainer { get; }

        [Option("--destination-container <CONTAINER_NAME>", Description = "name of destination container")]
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
