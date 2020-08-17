using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Exceptions.Commands
{
    class CommandNotFoundException : CliException
    {
        public CommandNotFoundException(string commandName) {
            CustomMessage = "The following command was not found: " + commandName;
        }
    }
}
