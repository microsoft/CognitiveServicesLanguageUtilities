using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.UtilitiesCommand
{
    [Command("utilities", Description = "shows help for utilities command")]
    [Subcommand(
        typeof(ParseCommand),
        typeof(ChunkCommand))]
    public class UtilitiesCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
