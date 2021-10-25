using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using System;
using System.Linq;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.SchemaValidation
{
    class CustomTextSchemaValidationService
    {
        public static void ValidateAppSchema(CustomTextSchema schema)
        {
            schema.EntityNames.ForEach(e => {
                if (e.Any(x => !char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
                {
                    var message = "Cognitive Search doesn't support spaces or special characters in entity names! please rename your entities and re-train your model";
                    throw new Exception(message);
                }
            });
        }
    }
}
