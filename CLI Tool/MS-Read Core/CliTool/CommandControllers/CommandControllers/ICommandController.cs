using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.CommandControllers
{
    interface ICommandController
    {
        public void ValidateArgs();
        public void Execute(string[] args);
    }
}
