using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation;
using CognitiveSearchIntegration.Tests.Configs;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveSearchIntegration.Tests.Runners.Validations
{
    public class CustomSkillsetValidationTest
    {
        public static TheoryData CustomSkillsetValidationServiceAsyncTestData()
        {
            var customTextConfigs = new CustomTextConfigModel()
            {
                Resource = new CustomTextResource()
                {
                    Endpoint = Secrets.Instance.CustomSkillsetCustomtextServiceEndpoint,
                    Key = Secrets.Instance.CustomSkillsetCustomtextServiceKey
                },
                Projects = new CustomTextProjects()
                {
                    EntityRecognition = new ProjectCredentials()
                    {
                        ProjectName = Secrets.Instance.CustomSkillsetCustomtextProjectName,
                        DeploymentName = Secrets.Instance.CustomSkillsetCustomtextDeploymentName,
                    }
                }
            };

            var selectedProjects = new SelectedProjects()
            {
                IsSelected_EntityRecognitionProject = false,
                IsSelected_SingleClassificationProject = false,
                IsSelected_MultiClassificationProject = false
            };

            return new TheoryData<string, CustomTextConfigModel, SelectedProjects, bool>
            {
                {
                    Secrets.Instance.CustomSkillsetAzureFunctionUrl,
                    customTextConfigs,
                    selectedProjects,
                    true
                },

                /*{
                    "https://customtextfunction20asd017202545.azurewebsites.net/api/customtext-extractor?code=kfOKcelrasdlvn64VobUn/CiU2asdAFdb2n/Tasdgknvx8IsAEMwoA==",
                    Secrets.Instance.CustomSkillsetCustomtextServiceEndpoint,
                    Secrets.Instance.CustomSkillsetCustomtextServiceKey,
                    Secrets.Instance.CustomSkillsetCustomtextProjectName,
                    Secrets.Instance.CustomSkillsetCustomtextDeploymentName,
                    false
                },
                {
                    Secrets.Instance.CustomSkillsetAzureFunctionUrl,
                    "lknlknasdlnlasjdnlajknsdljnasdljnasd",
                    Secrets.Instance.CustomSkillsetCustomtextServiceKey,
                    Secrets.Instance.CustomSkillsetCustomtextProjectName,
                    Secrets.Instance.CustomSkillsetCustomtextDeploymentName,
                    false
                },
                {
                    Secrets.Instance.CustomSkillsetAzureFunctionUrl,
                    Secrets.Instance.CustomSkillsetCustomtextServiceEndpoint,
                    "lojunaiosudbiaubsdasdasd",
                    Secrets.Instance.CustomSkillsetCustomtextProjectName,
                    Secrets.Instance.CustomSkillsetCustomtextDeploymentName,
                    false
                },
                {
                    Secrets.Instance.CustomSkillsetAzureFunctionUrl,
                    Secrets.Instance.CustomSkillsetCustomtextServiceEndpoint,
                    Secrets.Instance.CustomSkillsetCustomtextServiceKey,
                    "z9h8s6f664hf",
                    Secrets.Instance.CustomSkillsetCustomtextDeploymentName,
                    false
                },
                {
                    Secrets.Instance.CustomSkillsetAzureFunctionUrl,
                    Secrets.Instance.CustomSkillsetCustomtextServiceEndpoint,
                    Secrets.Instance.CustomSkillsetCustomtextServiceKey,
                    Secrets.Instance.CustomSkillsetCustomtextProjectName,
                    "iuygsdgulmae",
                    false
                },*/
            };
        }

        [Theory]
        [MemberData(nameof(CustomSkillsetValidationServiceAsyncTestData))]
        public async Task CustomSkillsetValidationServiceAsyncTest(
            string azureFunctionUrl,
            CustomTextConfigModel customTextConfigs,
            SelectedProjects selectedProjects,
            bool isValidCredentials)
        {
            var validationService = new SkillsetValidationService();

            if (isValidCredentials)
            {
                await validationService.ValidateCustomSkillsetConfigsAsync(azureFunctionUrl, customTextConfigs, selectedProjects);
            }
            else
            {
                await Assert.ThrowsAsync<Exception>(() => validationService.ValidateCustomSkillsetConfigsAsync(azureFunctionUrl, customTextConfigs, selectedProjects));
            }
        }
    }
}
