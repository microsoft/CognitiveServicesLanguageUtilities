using CliTool.Commands;
using CliTool.Commands.Config;
using CliTool.Configs.Consts;
using CliTool.Exceptions;
using CliTool.Services.Logger;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace CliTool
{
    [Command(Constants.ToolName)]
    [VersionOptionFromMember("--version")]
    [Subcommand(
        typeof(ParseCommand),
        typeof(PredictCommand),
        typeof(ConfigCommand))]
    class Program
    {
        public static void Main(string[] args) 
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            CommandLineApplication.Execute<Program>(args);
        }

        private int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        // TODO: Refactor unhandled exception handler
        // Where to place universal exception handler ?
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ILoggerService loggerService = new ConsoleLoggerService();
            Exception ex = e.ExceptionObject as Exception;
            loggerService.LogError(ex.InnerException);
            Environment.Exit(1);
        }
    }
}
