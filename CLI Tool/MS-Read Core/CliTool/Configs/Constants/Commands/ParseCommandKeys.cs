using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Configs.Constants.Commands
{
    class ParseCommand
    {
        public readonly string Name = "parse";
        public readonly ParseCommandKeys Keys = new ParseCommandKeys();
    }

    class ParseCommandKeys
    {
        public readonly string ParserFlag = "--parser";
        public readonly string SourceStorageFlag = "--source";
        public readonly string DestinationStorageFlag = "--destination";
        public readonly string ChunkFlag = "--chunking-type";
    }
}
