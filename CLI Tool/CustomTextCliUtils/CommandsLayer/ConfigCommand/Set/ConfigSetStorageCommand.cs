using McMaster.Extensions.CommandLineUtils;

namespace  Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("storage", Description = "sets configs for all storage services")]
    [Subcommand(
        typeof(ConfigSetBlobCommand),
        typeof(ConfigSetLocalCommand))]
    class ConfigSetStorageCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
