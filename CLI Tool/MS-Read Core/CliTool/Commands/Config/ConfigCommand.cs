using McMaster.Extensions.CommandLineUtils;


namespace CustomTextCliUtils.Commands.Config
{
    [Command("config", Description = "shows or sets app configs")]
    [Subcommand(
        typeof(ConfigShowCommand),
        typeof(ConfigSetCommand))]
    class ConfigCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
