using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Configs
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
