using Autofac;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Runner.DataStructures;
using FileFormatConverter.Runner.Services;
using System;

namespace FileFormatConverter.Runner
{
    public class FileConversionOperationRunner
    {
        private static LoggingService _logger = new LoggingService();
        private static ConfigurationService _configurationService = new ConfigurationService();
        public static void RunOperation(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {
            try
            {
                RunOperationInternal(sourceFilePath, sourceFileType, targetFilePath, targetFileType);
                _logger.LogSuccessMessage("File converted successfully!");
            }
            catch (Exception e)
            {
                _logger.LogUnhandledError(e);
            }
        }
        private static void RunOperationInternal(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {
            var container = _configurationService.RegisterServices(sourceFileType, targetFileType);

            using (var scope = container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<IFileConversionOrchestrator>();
                orchestrator.ConvertFile(sourceFilePath, targetFilePath);
            }
        }
    }
}
