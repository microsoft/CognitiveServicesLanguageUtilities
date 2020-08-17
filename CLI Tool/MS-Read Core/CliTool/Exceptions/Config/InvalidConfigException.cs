using System;
namespace CliTool.Exceptions.Parser
{
    class InvalidConfigException : CliException
    {
        public InvalidConfigException(string configName, string configValue, string serviceName)
        {
            CustomMessage = "Invalid " + serviceName + " Config: " + configName + " Value: " + configValue;
        }
    }
}
