using CustomTextCliUtils.Commands.Config.Set;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.Commands.Config
{
    [Command("set")]
    [Subcommand(
        typeof(ConfigSetStorage),
        typeof(ConfigSetParser))]
    class ConfigSetCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
