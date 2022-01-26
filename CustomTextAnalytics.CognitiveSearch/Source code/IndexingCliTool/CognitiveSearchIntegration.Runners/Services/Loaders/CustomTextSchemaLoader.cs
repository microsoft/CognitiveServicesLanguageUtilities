using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveSearchIntegration.Runners.Services.Loaders
{
    public class CustomTextSchemaLoader
    {
        public async Task<CustomTextSchema> LoadCustomTextAppSchema(
            CustomTextResource customTextResource,
            CustomTextProjects customTextProjects,
            SelectedProjects selectedProjects)
        {
            var client = new CustomTextAuthoringClient(customTextResource.Endpoint, customTextResource.Key);
            var result = new CustomTextSchema();

            // load extractors
            if (selectedProjects.IsSelected_EntityRecognitionProject)
            {
                var extractors = await LoadExtractors(client, customTextProjects.EntityRecognition.ProjectName);
                result.EntityNames = extractors.Select(e => e.Name).ToList();
            }

            // return result
            return result;
        }

        private async Task<Extractor[]> LoadExtractors(CustomTextAuthoringClient client, string projectName)
        {
            return await client.ExportProjectEntities(projectName);
        }
    }
}
