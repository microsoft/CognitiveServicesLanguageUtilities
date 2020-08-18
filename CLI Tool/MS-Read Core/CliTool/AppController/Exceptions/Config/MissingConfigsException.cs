using CliTool.Configs.Consts;
using CliTool.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.AppController.Exceptions.Config
{
    class MissingConfigsException : CliException
    {
        public MissingConfigsException()
            : base(ConstructMessage())
        { }

        public static string ConstructMessage()
        {
            return $"Please add the required configs using the command\n {Constants.ToolName} config set";
        }
    }
}
