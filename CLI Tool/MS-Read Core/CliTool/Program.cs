using CliTool.Commands;
using CliTool.Commands.Config;
using McMaster.Extensions.CommandLineUtils;

namespace CliTool
{
    [Command("app")]
    [VersionOptionFromMember("--version")]
    [Subcommand(
        typeof(ParseCommand),
        typeof(PredictCommand),
        typeof(ConfigCommand))]
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
