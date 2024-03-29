﻿using Newtonsoft.Json;
using System;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.Helpers
{
    public class JsonHandler
    {
        public static T DeserializeObject<T>(string value, string fileName = "")
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new Definitions.Exceptions.Serialization.JsonSerializationException(fileName);
            }
        }
    }
}
