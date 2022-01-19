using CustomSkillsetFunction.Models;
using CustomSkillsetFunction.Models.CustomTextPredictionService;
using CustomSkillsetFunction.Models.CustomTextPredictionService.Tasks;
using System.Collections.Generic;

namespace CustomSkillsetFunction.Helpers
{
    public class CustomTextTaskHelper
    {
        public static List<ITextAnalyticsTask> InitializeTargetTasks(Projects customTextProjects)
        {
            var targetTasks = new List<ITextAnalyticsTask>();

            // entity recognition
            var ERProject = customTextProjects.EntityRecognition;
            if (IsValidProjectCredentials(ERProject))
            {
                targetTasks.Add(new EntitiesRecognitionTask(ERProject.ProjectName, ERProject.DeploymentName));
            }

            // single classification
            var SCProject = customTextProjects.SingleClassification;
            if (IsValidProjectCredentials(SCProject))
            {
                targetTasks.Add(new SingleClassificationTask(SCProject.ProjectName, SCProject.DeploymentName));
            }

            // multi classification
            var MCProject = customTextProjects.MultiClassification;
            if (IsValidProjectCredentials(MCProject))
            {
                targetTasks.Add(new MultiClassificationTask(MCProject.ProjectName, MCProject.DeploymentName));
            }

            return targetTasks;
        }
        private static bool IsValidProjectCredentials(ProjectCredentials project)
        {
            if (string.IsNullOrEmpty(project.ProjectName) || string.IsNullOrEmpty(project.DeploymentName))
            {
                return false;
            }
            return true;
        }
    }
}
