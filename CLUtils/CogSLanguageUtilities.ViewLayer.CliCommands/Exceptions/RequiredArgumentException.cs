using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Exceptions
{
    public class RequiredArgumentException : Exception
    {
        public RequiredArgumentException(string argumentName) : base(CreateMessage(argumentName))
        { }

        private static string CreateMessage(string argumentName)
        {
            return string.Format("{0} is a required argument", argumentName);
        }
    }
}
