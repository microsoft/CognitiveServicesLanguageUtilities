
using Autofac;
using FileFormatConverter.Core.DataStructures.Enums;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Runner.Services;
using System;

namespace FileFormatConverter.Runner
{
    public class FileConversionOperationRunner
    {
        private static LoggingService _logger;
        public static void RunOperation(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {
            try
            {
                RunOperationInternal(sourceFilePath, sourceFileType, targetFilePath, targetFileType);
            }
            catch (Exception e)
            {

                _logger.LogUnhandledError(e);
            }
        }
        private static void RunOperationInternal(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {
            // build dependencies
            var container = DependencyInjectionService.BuildDependencies(sourceFileType, targetFileType);

            // run
            using (var scope = container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<FileConversionOrchestrator>();
                orchestrator.ConvertFile(sourceFilePath, targetFilePath);
            }
        }
    }
}
