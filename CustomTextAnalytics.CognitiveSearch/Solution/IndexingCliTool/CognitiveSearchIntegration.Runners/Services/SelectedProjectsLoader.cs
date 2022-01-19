
using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using System;

namespace CognitiveSearchIntegration.Runners.Services
{
    class SelectedProjectsLoader
    {
        public static SelectedProjects LoadSelectedProjects(CustomTextProjects projects)
        {
            var ERP = IsValidProjectCredentials(projects.EntityRecognition);
            var SCP = IsValidProjectCredentials(projects.SingleClassification);
            var MCP = IsValidProjectCredentials(projects.MultiClassification);
            if (!ERP && !SCP && !MCP)
            {
                throw new Exception("Please Provide One or More Custom Text Projects!");
            }
            return new SelectedProjects()
            {
                IsSelected_EntityRecognitionProject = ERP,
                IsSelected_SingleClassificationProject = SCP,
                IsSelected_MultiClassificationProject = MCP
            };
        }
        public static bool IsValidProjectCredentials(ProjectCredentials project)
        {
            if (string.IsNullOrEmpty(project.ProjectName) || string.IsNullOrEmpty(project.DeploymentName))
            {
                return false;
            }
            return true;
        }
    }
}
