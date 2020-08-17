using Azure;
using CliTool.Exceptions.Storage;
using System;
using System.IO;

namespace CliTool.Exceptions
{
    class CliException : Exception
    {
        public string CustomMessage { get; set; }
    }
}
