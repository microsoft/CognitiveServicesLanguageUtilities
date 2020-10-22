using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts;
using System.IO;
using System.Reflection;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.CommandHelp
{
    public class CustomHelpTextGenerator : IHelpTextGenerator
    {
        private static Assembly _assembly = Assembly.GetExecutingAssembly();
        public void Generate(CommandLineApplication application, TextWriter output)
        {
            // get called cli command
            var command = application.Name;

            // get version info
            var version = _assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var toolName = Constants.ToolName;
            var description = "Azure Cognitive Services Language Utilities";
            var license = "Copyright(c) Microsoft Corporation.\nLicensed under the MIT License.";
            var headerMessage = string.Format("{0}\n{1}\nVersion: {2}\n{3}\n\n", toolName, description, version, license);


            // load command text
            Stream stream = _assembly.GetManifestResourceStream($"Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.CommandHelp.HelpText.{command}");
            var helpText = new StreamReader(stream);

            // output message
            output.WriteLine(headerMessage);
            output.WriteLine(helpText.ReadToEnd());
        }
    }
}