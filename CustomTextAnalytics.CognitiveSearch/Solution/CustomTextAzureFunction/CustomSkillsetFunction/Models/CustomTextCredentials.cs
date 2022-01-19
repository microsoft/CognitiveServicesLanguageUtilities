namespace CustomSkillsetFunction.Models
{
    public class CustomTextCredentials
    {
        public ResourceCredentials ResourceCredentials { get; set; }
        public Projects Projects { get; set; }
    }
    public class ResourceCredentials
    {
        public string EndpointUrl { get; set; }
        public string Key { get; set; }
    }
    public class Projects
    {
        public ProjectCredentials EntityRecognition { get; set; }
        public ProjectCredentials SingleClassification { get; set; }
        public ProjectCredentials MultiClassification { get; set; }
    }
    public class ProjectCredentials
    {
        public string ProjectName { get; set; }
        public string DeploymentName { get; set; }
    }
}