
using CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions;
using System;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions
{
    public class CliException : Exception
    {
        public CliExceptionCode ExceptionCode;

        public CliException(CliExceptionCode exceptionCode, string message)
            : base(message)
        {
            ExceptionCode = exceptionCode;
        }
    }
}
