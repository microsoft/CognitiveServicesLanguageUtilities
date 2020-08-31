using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("prediction", Description = "sets configs for prediction")]
    class ConfigSetPredictionCommand
    {
        [Option(CommandOptionTemplate.PredictionCustomTextKey, Description = "custom text app prediction resource key")]
        public string CustomTextKey { get; }
        [Option(CommandOptionTemplate.PredictionEndpointUrl, Description = "custom text app prediction resource endpoint url")]
        public string EndpointUrl { get; }
        [Option(CommandOptionTemplate.PredictionAppId, Description = "custom text app id")]
        public string AppId { get; }
        [Option(CommandOptionTemplate.PredictionVersionId, Description = "custom text app version id")]
        public string VersionId { get; }

        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.SetPredictionConfigs(CustomTextKey, EndpointUrl, AppId, VersionId);
            }

            return 1;
        }
    }
}
