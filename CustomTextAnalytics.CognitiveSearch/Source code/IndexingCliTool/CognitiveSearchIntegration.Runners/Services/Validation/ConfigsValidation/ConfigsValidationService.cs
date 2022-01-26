using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using System.Threading.Tasks;

namespace CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation
{
    class ConfigsValidationService
    {
        private BlobStorageValidationService _blobStorageValidationService = new BlobStorageValidationService();
        private SkillsetValidationService _skillsetValidationService = new SkillsetValidationService();
        private CognitiveSearchValidationService _cognitiveSearchValidationService = new CognitiveSearchValidationService();
        public async Task ValidateAppConfigs(ConfigModel appConfigs, SelectedProjects selectedProjects)
        {
            await _blobStorageValidationService.ValidateBlobConfigsAsync(
                appConfigs.BlobStorage.ConnectionString,
                appConfigs.BlobStorage.ContainerName);

            await _cognitiveSearchValidationService.ValidateCognitiveSearchConfigs(
                    appConfigs.CognitiveSearch.EndpointUrl,
                    appConfigs.CognitiveSearch.ApiKey);

            await _skillsetValidationService.ValidateCustomSkillsetConfigsAsync(
                    appConfigs.AzureFunction.FunctionUrl,
                    appConfigs.CustomText,
                    selectedProjects);
        }
    }
}
