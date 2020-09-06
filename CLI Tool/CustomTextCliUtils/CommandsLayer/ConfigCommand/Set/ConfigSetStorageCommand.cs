using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
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
