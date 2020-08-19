
using System;

namespace CustomTextCliUtils.AppController.Exceptions
{
    class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        { }
    }
}
