using Autofac;
using FileFormatConverter.Core;
using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Core.Services;
using FileFormatConverter.Runner.DataStructures;
using System;

namespace FileFormatConverter.Runner.Services
{
    public class DependencyInjectionService
    {
        public static IContainer RegisterServices(FileType sourceFileType, FileType targetFileType)
        {
            var builder = new ContainerBuilder();

            RegsiterFileHandler(builder);
            RegisterModelSerializer(builder, sourceFileType);
            RegisterModelConverter(builder, sourceFileType, targetFileType);
            RegisterModelSerializer(builder, targetFileType);
            RegisterConversionOrchestrator(builder, sourceFileType, targetFileType);

            return builder.Build();
        }

        private static void RegsiterFileHandler(ContainerBuilder builder)
        {
            builder.RegisterType<LocalFileHandlerService>().As<IFileHandler>();
        }

        private static void RegisterModelSerializer(ContainerBuilder builder, FileType fileType)
        {
            builder.RegisterType<JsonlModelSerializerService>().As<IModelSerializer<JsonlFileModel>>();
            builder.RegisterType<CustomTextEntitiesFileModelSerializer>().As<IModelSerializer<CustomEntitiesFileModel>>();
        }

        private static void RegisterModelConverter(ContainerBuilder builder, FileType sourceType, FileType targetType)
        {

            builder.RegisterType<JsonlModelConversionService>().As<IModelConverter<JsonlFileModel, CustomEntitiesFileModel>>();
        }

        private static void RegisterConversionOrchestrator(ContainerBuilder builder, FileType sourceType, FileType targetType)
        {
            var sourceFileType = GetModelType(sourceType);
            var targetFileType = GetModelType(targetType);
            var openType = typeof(FileConversionOrchestrator<,>);
            var closedType = openType.MakeGenericType(sourceFileType, targetFileType);

            builder.RegisterGeneric

            builder.Register(c =>
            {
                c.
                var instance = Activator.CreateInstance(closedType,
                    c.Resolve<IFileHandler>(),
                    c.Resolve<IModelSerializer<JsonlFileModel>>(),
                    c.Resolve<IModelConverter<JsonlFileModel, CustomEntitiesFileModel>>(),
                    c.Resolve<IModelSerializer<CustomEntitiesFileModel>>()
                    );
            }).As<IFileConversionOrchestrator<JsonlFileModel, CustomEntitiesFileModel>>();
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
    }
}
