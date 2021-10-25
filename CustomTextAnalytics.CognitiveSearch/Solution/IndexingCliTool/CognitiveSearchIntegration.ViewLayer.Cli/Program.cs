using McMaster.Extensions.CommandLineUtils;
using Microsoft.CognitiveSearchIntegration.Core.Services.Logger;
using Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Commands;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using System;
using System.Reflection;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli
{
    [Command("cognitivesearch")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(IndexCommand))]
    class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            CommandLineApplication.Execute<Program>(args);
        }

        private static string GetVersion()
           => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            var loggerService = new ConsoleLoggerService();
            var ex = e.ExceptionObject as Exception;
            if (ex is CliException)
            {
                loggerService.LogUnhandledError(ex);
            }
            else if (ex?.InnerException is CliException)
            {
                loggerService.LogUnhandledError(ex.InnerException);
            }
            else if (ex?.InnerException?.InnerException is CliException)
            {
                loggerService.LogUnhandledError(ex.InnerException.InnerException);
            }
            else
            {
                loggerService.LogUnhandledError(ex);
            }
            Environment.Exit(1);
        }

    }
}
