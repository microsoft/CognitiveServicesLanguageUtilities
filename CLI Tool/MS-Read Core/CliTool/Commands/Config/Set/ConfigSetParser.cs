using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Config.Set
{
    [Command("parser")]
    [Subcommand(
        typeof(ConfigSetMsReadCommand))]
    class ConfigSetParser
    {
        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}
