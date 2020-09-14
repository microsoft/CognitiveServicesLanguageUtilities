using Autofac;
using Microsoft.CustomTextCliUtils.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("textanalytics", Description = "sets configs for textanalytics")]
    public class ConfigSetTextAnalyticsCommand
    {
        [Option(CommandOptionTemplate.TextAnalyticsAzureResourceKey, Description = "text analytics azure resource key")]
        public string AzureResourceKey { get; }
        [Option(CommandOptionTemplate.TextAnalyticsAzureResourceEndpoint, Description = "text analytics azure resource endpoint")]
        public string AzureResourceEndpoint { get; }
        [Option(CommandOptionTemplate.TextAnalyticsDefaultLanguage, Description = "text analytics prediction default language")]
        public string DefaultLanguage { get; }
        [Option(CommandOptionTemplate.TextAnalyticsEnableSentiment, optionType: CommandOptionType.SingleValue, Description = "text analytics enable sentiment by default")]
        public bool? EnableSentimentByDefault { get; }
        [Option(CommandOptionTemplate.TextAnalyticsEnableNer, optionType: CommandOptionType.SingleValue, Description = "text analytics enable NER by default")]
        public bool? EnableNerByDefault { get; }
        [Option(CommandOptionTemplate.TextAnalyticsEnableKeyphrase, optionType: CommandOptionType.SingleValue, Description = "text analytics enable keyphrase by default")]
        public bool? EnableKeyphraseByDefault { get; }

        private async Task OnExecuteAsync(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                await controller.SetTextAnalyticsConfigsAsync(AzureResourceKey, AzureResourceEndpoint, DefaultLanguage, EnableSentimentByDefault, EnableNerByDefault, EnableKeyphraseByDefault);
            }
        }
    }
}
