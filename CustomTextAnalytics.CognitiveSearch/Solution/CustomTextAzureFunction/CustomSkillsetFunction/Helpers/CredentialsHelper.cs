using CustomSkillsetFunction.Configs;
using CustomSkillsetFunction.Models;
using Microsoft.AspNetCore.Http;

namespace CustomSkillsetFunction.Helpers
{
    public class CredentialsHelper
    {
        public static CustomTextCredentials GetProjectCredentials(IHeaderDictionary headers)
        {
            return new CustomTextCredentials()
            {
                ResourceCredentials = new ResourceCredentials()
                {
                    EndpointUrl = headers[Constants.CustomTextResourceEndpointHeader],
                    Key = headers[Constants.CustomTextResourceKeyHeader]
                },
                Projects = new Projects()
                {
                    EntityRecognition = new ProjectCredentials()
                    {
                        ProjectName = headers[Constants.EntityRecognitionProjectNameHeader],
                        DeploymentName = headers[Constants.EntityRecognitionDeploymentNameHeader],
                    },
                    SingleClassification = new ProjectCredentials()
                    {
                        ProjectName = headers[Constants.SingleClassificationProjectNameHeader],
                        DeploymentName = headers[Constants.SingleClassificationDeploymentNameHeader],
                    },
                    MultiClassification = new ProjectCredentials()
                    {
                        ProjectName = headers[Constants.MultiClassificationProjectNameHeader],
                        DeploymentName = headers[Constants.MultiClassificationDeploymentNameHeader],
                    }
                }
            };
        }
    }
}
