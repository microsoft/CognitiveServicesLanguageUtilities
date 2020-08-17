
using CliTool.Commands;
using CliTool.Configs.Constants;
using CliTool.Exceptions.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace CliTool
{
    [Command("app")]
    [VersionOptionFromMember("--version")]
    [Subcommand(
        typeof(ParseCommand),
        typeof(PredictCommand))]
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        protected int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
