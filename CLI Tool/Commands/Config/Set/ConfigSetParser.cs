using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config.Set
{
    [Command("parser", Description = "sets configs for all parsers")]
    [Subcommand(
        typeof(ConfigSetMsReadCommand))]
    class ConfigSetParser
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
