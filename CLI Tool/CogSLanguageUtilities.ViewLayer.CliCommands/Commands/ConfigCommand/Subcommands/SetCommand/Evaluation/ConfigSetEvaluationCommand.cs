using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using Microsoft.CustomTextCliUtils.Configs;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("evaluation", Description = "sets configs for evaluation labeled examples app")]
    public class ConfigSetEvaluationCommand
    {
        [Option(CommandOptionTemplate.EvaluationLabeledExamplesAppAzureResourceKey, Description = "labeled examples app authoring resource key")]
        public string AzureResourceKey { get; }
        [Option(CommandOptionTemplate.EvaluationLabeledExamplesAppAzureResourceEndpoint, Description = "labeled examples app authoring resource endpoint url")]
        public string AzureResourceEndpoint { get; }
        [Option(CommandOptionTemplate.EvaluationLabeledExamplesAppId, Description = "labeled examples app id")]
        public string AppId { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigsController>();
                await controller.SetEvaluationConfigsAsync(AzureResourceKey, AzureResourceEndpoint, AppId);
            }
        }
    }
}
