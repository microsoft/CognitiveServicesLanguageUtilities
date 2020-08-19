using CustomTextCliUtils.Configs.Consts;

namespace CustomTextCliUtils.AppController.Exceptions.Parser
{
    class MsReadUnauthorizedException : CliException
    {
        public MsReadUnauthorizedException(string cognitiveServicesKey, string cognitiveServicesEndpoint)
            : base(ConstructMessage(cognitiveServicesKey, cognitiveServicesEndpoint))
        { }

        private static string ConstructMessage(string cognitiveServicesKey, string cognitiveServicesEndpoint)
        {
            return "Unauthorized access to Azure Cognitive Services \ncheck " + Constants.ConfigKeys.CognitiveServicesKey + ": "
                + cognitiveServicesKey + " or " + Constants.ConfigKeys.CognitiveServicesEndpoint + ": " + cognitiveServicesEndpoint;
        }
    }
}
