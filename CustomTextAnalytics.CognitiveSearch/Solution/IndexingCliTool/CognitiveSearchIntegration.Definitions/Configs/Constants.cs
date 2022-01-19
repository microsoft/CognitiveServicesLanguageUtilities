namespace Microsoft.CognitiveSearchIntegration.Definitions.Configs
{
    public class Constants
    {
        // skillset response
        public static readonly string SkillsetResponseEntitiesKey = "entities";
        public static readonly string SkillsetResponseSingleClassKey = "classification";
        public static readonly string SkillsetResponseMultiClassKey = "multiclassification";

        // skillset request credentials
        public static readonly string CustomTextResourceEndpointHeader = "resourceEndpointUrl";
        public static readonly string CustomTextResourceKeyHeader = "resourceKey";

        public static readonly string EntityRecognitionProjectNameHeader = "entityRecognitionProjectName";
        public static readonly string EntityRecognitionDeploymentNameHeader = "entityRecognitionDeploymentName";

        public static readonly string SingleClassificationProjectNameHeader = "singleClassificationProjectName";
        public static readonly string SingleClassificationDeploymentNameHeader = "singleClassificationDeploymentName";

        public static readonly string MultiClassificationProjectNameHeader = "multiClassificationProjectName";
        public static readonly string MultiClassificationDeploymentNameHeader = "multiClassificationDeploymentName";

        // index column names
        public static readonly string SearchIndexSingleClassColumnName = "single_classification";
        public static readonly string SearchIndexMultiClassColumnName = "multi_classification";

    }
}
