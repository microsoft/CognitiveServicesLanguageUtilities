using Autofac;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Runner.DataStructures;
using FileFormatConverter.Runner.Services;
using System;

namespace FileFormatConverter.Runner
{
    public class FileConversionOperationRunner
    {
        private static readonly LoggingService _logger = new LoggingService();
        private static readonly ConfigurationService _configurationService = new ConfigurationService();
        public static void RunOperation(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {
            try
            {
                RunOperationpublic(sourceFilePath, sourceFileType, targetFilePath, targetFileType);
                _logger.LogSuccess("File converted successfully!");
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
        }
        private static void RunOperationpublic(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
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
