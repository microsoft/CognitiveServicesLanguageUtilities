using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Commands.RunPipelineCommand
{
    [Command("pipeline")]
    [Subcommand(
    typeof(RunCommand))]
    class PipelineCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
