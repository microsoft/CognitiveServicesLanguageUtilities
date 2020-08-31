using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    class BadRequestException : CliException
    {
        public BadRequestException(string message)
            : base(message)
        { }
    }
}
