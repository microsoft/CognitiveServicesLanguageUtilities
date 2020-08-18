using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config.Set
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
