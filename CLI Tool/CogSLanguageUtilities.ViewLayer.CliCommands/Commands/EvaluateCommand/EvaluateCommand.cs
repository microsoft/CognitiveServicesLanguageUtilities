using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Commands.EvaluateCommand
{
    [Command("evaluate", Description = "")]
    public class EvaluateCommand
    {
        private async Task OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
