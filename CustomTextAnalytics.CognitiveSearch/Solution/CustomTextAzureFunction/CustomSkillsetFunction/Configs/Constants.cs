namespace CustomSkillsetFunction.Configs
{
    public class Constants
    {
        // request credentials
        public static readonly string CustomTextResourceEndpointHeader = "resourceEndpointUrl";
        public static readonly string CustomTextResourceKeyHeader = "resourceKey";

        public static readonly string EntityRecognitionProjectNameHeader = "entityRecognitionProjectName";
        public static readonly string EntityRecognitionDeploymentNameHeader = "entityRecognitionDeploymentName";

        public static readonly string SingleClassificationProjectNameHeader = "singleClassificationProjectName";
        public static readonly string SingleClassificationDeploymentNameHeader = "singleClassificationDeploymentName";

        public static readonly string MultiClassificationProjectNameHeader = "multiClassificationProjectName";
        public static readonly string MultiClassificationDeploymentNameHeader = "multiClassificationDeploymentName";


        // response keys
        public static readonly string ResponseEntitiesKey = "entities";
        public static readonly string ResponseSingleClassKey = "classification";
        public static readonly string ResponseMultiClassKey = "multiclassification";
    }
}
