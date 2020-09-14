using McMaster.Extensions.CommandLineUtils;
using Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set;

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
