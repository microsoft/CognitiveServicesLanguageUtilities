using Newtonsoft.Json;

namespace CognitiveSearchIntegration.Runners.Models.ConfigsModel
{
    public class CustomTextConfigModel
    {
        [JsonProperty("resource")]
        public CustomTextResource Resource { get; set; }

        [JsonProperty("projects")]
        public CustomTextProjects Projects { get; set; }
    }
    public class CustomTextResource
    {
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
    public class CustomTextProjects
    {
        [JsonProperty("entityRecognition")]
        public ProjectCredentials EntityRecognition { get; set; }

        [JsonProperty("singleClassification")]
        public ProjectCredentials SingleClassification { get; set; }

        [JsonProperty("multiClassification")]
        public ProjectCredentials MultiClassification { get; set; }
    }

    public class ProjectCredentials
    {
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("deploymentName")]
        public string DeploymentName { get; set; }
    }


}
