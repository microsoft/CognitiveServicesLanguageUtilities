using Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set;
using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand
{
    [Command("set", Description = "sets app configs")]
    [Subcommand(
        typeof(ConfigSetStorageCommand),
        typeof(ConfigSetParserCommand),
        typeof(ConfigSetChunkerCommand),
        typeof(ConfigSetPredictionCommand))]
    public class ConfigSetCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
