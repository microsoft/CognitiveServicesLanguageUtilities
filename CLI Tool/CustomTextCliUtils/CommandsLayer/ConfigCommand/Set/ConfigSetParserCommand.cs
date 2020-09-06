using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.CustomTextCliUtils.CommandsLayer.ConfigCommand.Set
{
    [Command("parser", Description = "sets configs for all parsers")]
    [Subcommand(
        typeof(ConfigSetMsReadCommand))]
    public class ConfigSetParserCommand
    {
        private void OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
        }
    }
}
