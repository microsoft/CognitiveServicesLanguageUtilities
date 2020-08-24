
using System;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions
{
    class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        { }
    }
}
