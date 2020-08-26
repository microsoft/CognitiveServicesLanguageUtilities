using System;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions
{
    public class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        { }
    }
}
