using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.ConfigCommand
{
    [Command("storage", Description = "sets configs for all storage services")]
    [Subcommand(
        typeof(ConfigSetBlobCommand),
        typeof(ConfigSetLocalCommand))]
    public class ConfigSetStorageCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
