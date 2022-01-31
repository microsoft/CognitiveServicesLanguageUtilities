using McMaster.Extensions.CommandLineUtils;
using Microsoft.CognitiveSearchIntegration.Cli.Commands;
using System.Reflection;

namespace Microsoft.CognitiveSearchIntegration.Cli
{
    [Command("")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ConvertFileCommand))]
    class Start
    {
        public static void Main(string[] args)
        {
            CommandLineApplication.Execute<Start>(args);
        }

        private static string GetVersion()
           => typeof(Start).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
