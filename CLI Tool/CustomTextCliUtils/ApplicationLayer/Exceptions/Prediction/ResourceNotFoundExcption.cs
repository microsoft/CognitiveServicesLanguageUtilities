using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    public class ResourceNotFoundExcption : CliException
    {
        public ResourceNotFoundExcption(string message)
            : base(message)
        { }
    }
}
