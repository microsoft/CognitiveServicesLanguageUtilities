using System;

namespace CognitiveSearchIntegration.Common.Exceptions
{
    public class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        { }
    }
}
