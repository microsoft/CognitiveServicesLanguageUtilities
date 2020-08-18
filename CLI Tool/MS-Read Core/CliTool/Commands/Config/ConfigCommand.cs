using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config
{
    [Command("config")]
    [Subcommand(
        typeof(ConfigShowCommand),
        typeof(ConfigSetCommand))]
    class ConfigCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
