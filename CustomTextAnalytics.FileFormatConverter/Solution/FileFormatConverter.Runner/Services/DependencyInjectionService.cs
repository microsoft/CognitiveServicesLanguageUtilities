using Autofac;
using FileFormatConverter.Core.DataStructures.Enums;
using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Orchestrators;
using FileFormatConverter.Services;
using System;

namespace FileFormatConverter.Runner.Services
{
    public class DependencyInjectionService
    {
        private static IModelSerializer<TModel> GetModelSerializer<TModel>(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.JSONL:
                    return new JsonlModelSerializerService();
                case FileType.CT_ENTITIES:
                    return new CustomTextEntitiesFileModelSerializer();
                default:
                    return null;
            }
        }

        private static Type GetModelType(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.JSONL:
                    return typeof(JsonlFileModel);
                case FileType.CT_ENTITIES:
                    return typeof(CustomEntitiesFileModel);
                default:
                    return null;
            }
        }

        public static IContainer BuildDependencies(FileType sourceFileType, FileType targetFileType)
        {
            var builder = new ContainerBuilder();

            // register services
            // builder.RegisterType<ConsoleLoggerService>().As<ILoggerService>();

            var fileHandlingService = new LocalFileHandlerService();

            var sourceModelType = GetModelType(sourceFileType);
            var targetModelType = GetModelType(targetFileType);

            var sourceModelSerializerService = GetModelSerializer(sourceFileType);
            var targetModelSerializerService = GetModelSerializer(targetFileType);

            builder.Register(c =>
            {
                return new FileConversionOrchestrator<sourceModelType, targetModelType>(
                    fileHandlingService,
                    sourceModelSerializerService,
                    null,
                    targetModelSerializerService);
            }).As<IFileConversionOrchestrator>();

            builder.Register(c =>
            {
                return new CognitiveSearchService(appConfigs.CognitiveSearch.EndpointUrl, appConfigs.CognitiveSearch.ApiKey);
            }).As<CognitiveSearchService>();

            builder.Register(c =>
            {
                return new IndexingOrchestrator(
                    c.Resolve<CognitiveSearchSchemaCreatorService>(),
                    c.Resolve<CognitiveSearchService>(),
                    c.Resolve<ILoggerService>(),
                    appConfigs);
            });
            return builder.Build();
        }
    }
}
