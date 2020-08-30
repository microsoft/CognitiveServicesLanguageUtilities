using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    class ResourceNotFoundExcption : CliException
    {
        public ResourceNotFoundExcption(string message)
            : base(message)
        { }
    }
}
