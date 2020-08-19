using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config.Set
{
    [Command("storage")]
    [Subcommand(
        typeof(ConfigSetBlobCommand),
        typeof(ConfigSetLocalCommand))]
    class ConfigSetStorage
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
