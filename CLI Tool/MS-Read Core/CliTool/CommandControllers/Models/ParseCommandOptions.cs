using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Commands.Models
{
    class ParseCommandOptions
    {
        [Option(Description = "The subject")]
        public string Subject { get; }

        [Option(ShortName = "n")]
        public int Count { get; }
    }
}
