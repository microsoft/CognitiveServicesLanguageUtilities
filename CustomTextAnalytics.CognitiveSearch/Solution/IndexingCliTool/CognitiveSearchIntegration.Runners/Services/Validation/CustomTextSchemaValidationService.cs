using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using System;
using System.Linq;

namespace CognitiveSearchIntegration.Runners.Services.Validation
{
    class CustomTextSchemaValidationService
    {
        public void ValidateSchema(CustomTextSchema schema, CustomTextProjects projects, SelectedProjects selectedProjects)
        {
            var result = ValidateSchemaInternal(schema, projects, selectedProjects);
            if (result == false)
            {
                throw new Exception("Cognitive Search doesn't support spaces or special characters in entity names! please rename your entities and re-train your model");
            }
        }
        private bool ValidateSchemaInternal(CustomTextSchema schema, CustomTextProjects projects, SelectedProjects selectedProjects)
        {
            var result = true;
            if (selectedProjects.IsSelected_EntityRecognitionProject)
            {
                result &= ValidateEntityNames(schema);
            }

            return result;
        }
        private bool ValidateEntityNames(CustomTextSchema schema)
        {
            var result = true;
            schema.EntityNames.ForEach(e =>
            {
                if (e.Any(x => !char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
                {
                    result = false;
                }
            });
            return result;
        }
    }
}
