using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    class UnauthorizedRequestException : CliException
    {
        public UnauthorizedRequestException(string url, string key)
            : base(ConstructMessage(url, key))
        { }

        public static string ConstructMessage(string url, string key)
        {
            return $"Unauthorized Request to {url} \nusing key {key}";
        }
    }
}
