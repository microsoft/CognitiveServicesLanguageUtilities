using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config.Set
{
    [Command("local")]
    class ConfigSetLocalCommand
    {
        [Option("--source-dir <ABSOLUTE_PATH>")]
        public string SourceDirectory { get; }

        [Option("--destination-dir <ABSOLUTE_PATH>")]
        public string DestinationDirectory { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetLocalStorageConfigs(SourceDirectory, DestinationDirectory);
            }

            return 1;
        }
    }
}
