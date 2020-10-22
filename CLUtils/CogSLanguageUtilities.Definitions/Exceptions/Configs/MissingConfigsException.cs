using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;

namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Configs
{
    public class MissingConfigsException : CliException
    {
        public MissingConfigsException()
            : base(ConstructMessage())
        { }

        public static string ConstructMessage()
        {
            return $"Please add the required configs using the command\n{Constants.ToolName} config set";
        }
    }
}
