using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Exceptions
{
    class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string argumentName) : base(CreateMessage(argumentName))
        { }

        private static string CreateMessage(string argumentName)
        {
            return string.Format("{0} is an invalid argument", argumentName);
        }
    }
}
