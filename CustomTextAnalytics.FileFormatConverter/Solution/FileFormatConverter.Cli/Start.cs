using McMaster.Extensions.CommandLineUtils;
using Microsoft.FileFormatConverter.Cli.Commands;
using System.Reflection;

namespace Microsoft.FileFormatConverter.Cli
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
