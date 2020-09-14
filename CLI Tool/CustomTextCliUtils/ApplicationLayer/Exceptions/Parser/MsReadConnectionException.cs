using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Parser
{
    public class MsReadConnectionException : CliException
    {
        public MsReadConnectionException(string message)
            : base(ConstructMessage(message))
        { }

        private static string ConstructMessage(string message)
        {
            return $"Connection to Azure Cognitive Services failed with message: \"{message}\" \nCheck MsRead configs" + 
                $"{ConfigKeys.MSReadAzureResourceKey} and {ConfigKeys.MSReadAzureResourceEndpoint}";
        }
    }
}
