
using Autofac;
using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Runner.DataStructures;
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
                throw e;
            }
        }
        private static void RunOperationInternal(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {
            // build dependencies
            var container = DependencyInjectionService.RegisterServices(sourceFileType, targetFileType);

            // run
            using (var scope = container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<IFileConversionOrchestrator<JsonlFileModel, CustomEntitiesFileModel>>();
                orchestrator.ConvertFile(sourceFilePath, targetFilePath);
            }
        }
    }
}
