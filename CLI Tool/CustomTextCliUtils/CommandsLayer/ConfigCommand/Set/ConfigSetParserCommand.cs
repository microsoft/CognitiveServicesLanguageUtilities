using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("parser", Description = "sets configs for all parsers")]
    [Subcommand(
        typeof(ConfigSetMsReadCommand))]
    class ConfigSetParserCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
