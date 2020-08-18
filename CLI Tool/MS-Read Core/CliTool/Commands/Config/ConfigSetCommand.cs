using CliTool.Commands.Config.Set;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config
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
