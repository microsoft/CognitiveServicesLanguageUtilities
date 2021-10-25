// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using McMaster.Extensions.CommandLineUtils;
using Microsoft.IAPUtilities.Core.Services.Logger;
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Configs.Consts;
using Microsoft.IAPUtilities.Definitions.Exceptions;
using Microsoft.IAPUtilities.ViewLayer.CliCommands.Commands.RunPipelineCommand;
using System;
using System.Reflection;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands
{
    [Command(Constants.ToolName)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(PipelineCommand))]
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            CommandLineApplication.Execute<Program>(args);
        }

        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        // TODO: Refactor unhandled exception handler
        // Where to place universal exception handler ?
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ILoggerService loggerService = new ConsoleLoggerService();
            Exception ex = e.ExceptionObject as Exception;
            if (ex is CliException)
            {
                loggerService.LogError(ex);
            }
            else if (ex?.InnerException is CliException)
            {
                loggerService.LogError(ex.InnerException);
            }
            else if (ex?.InnerException?.InnerException is CliException)
            {
                loggerService.LogError(ex.InnerException.InnerException);
            }
            else
            {
                loggerService.LogError(ex);
            }
            Environment.Exit(1);
        }
    }
}
