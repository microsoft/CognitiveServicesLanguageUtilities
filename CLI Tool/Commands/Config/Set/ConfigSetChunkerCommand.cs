using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.ServiceControllers.Controllers;
using McMaster.Extensions.CommandLineUtils;
using CustomTextCliUtils.Configs.Consts;
using System.ComponentModel.DataAnnotations;

namespace CustomTextCliUtils.Commands.Config.Set
{
    [Command("chunker", Description = "sets configs for chunker")]
    class ConfigSetChunkerCommand
    {
        [Required]
        [Option(CommandOptionTemplate.ChunkerCharLimit, Description = "name of destination container")]
        public int CharLimit { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetChunkerConfigs(CharLimit);
            }

            return 1;
        }
    }
}
