using CliTool.Configs;
using CliTool.Configs.Consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Exceptions.Parser
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
