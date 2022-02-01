using FileFormatConverter.Core;
using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Services;
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
            /*var container = DependencyInjectionService.RegisterServices(sourceFileType, targetFileType);

            using (var scope = container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<IFileConversionOrchestrator<JsonlFileModel, CustomEntitiesFileModel>>();
                orchestrator.ConvertFile(sourceFilePath, targetFilePath);
            }*/

            var orchestrator = new FileConversionOrchestrator<JsonlFileModel, CustomEntitiesFileModel>(
                new LocalFileHandlerService(),
                new JsonlModelSerializerService(),
                new JsonlModelConversionService(),
                new CustomTextEntitiesFileModelSerializer());

            orchestrator.ConvertFile(sourceFilePath, targetFilePath);
        }
    }
}
