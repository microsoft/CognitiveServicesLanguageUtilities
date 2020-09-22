using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("set", Description = "sets app configs")]
    [Subcommand(
        typeof(ConfigSetStorageCommand),
        typeof(ConfigSetParserCommand),
        typeof(ConfigSetChunkerCommand),
        typeof(ConfigSetCustomTextCommand),
        typeof(ConfigSetTextAnalyticsCommand))]
    public class ConfigSetCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
