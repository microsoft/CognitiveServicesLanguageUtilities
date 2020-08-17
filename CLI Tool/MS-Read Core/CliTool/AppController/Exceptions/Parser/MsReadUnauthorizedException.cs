using CliTool.Configs;
using CliTool.Configs.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Exceptions.Parser
{
    class MsReadUnauthorizedException : CliException
    {
        public MsReadUnauthorizedException(string cognitiveServicesKey, string cognitiveServicesEndpoint)
        {
            CustomMessage = "Unauthorized access to Azure Cognitive Services \ncheck " + Constants.ConfigKeys.CognitiveServicesKey + ": " 
                + cognitiveServicesKey + " or " + Constants.ConfigKeys.CognitiveServicesEndpoint + ": " + cognitiveServicesEndpoint;
        }
    }
}
