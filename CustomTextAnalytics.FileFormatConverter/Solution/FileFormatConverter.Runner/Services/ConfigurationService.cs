using Autofac;
using FileFormatConverter.Core;
using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Core.Services;
using FileFormatConverter.Runner.DataStructures;

namespace FileFormatConverter.Runner.Services
{
    public class ConfigurationService
    {
        public IContainer RegisterServices(FileType sourceFileType, FileType targetFileType)
        {
            var builder = new ContainerBuilder();

            RegsiterFileHandler(builder);
            RegisterModelSerializers(builder, sourceFileType);
            RegisterModelConverters(builder, sourceFileType, targetFileType);
            RegisterModelSerializers(builder, targetFileType);
            RegisterConversionOrchestrator(builder, sourceFileType, targetFileType);

            return builder.Build();
        }

        private void RegsiterFileHandler(ContainerBuilder builder)
        {
            builder.RegisterType<LocalFileHandlerService>().As<IFileHandler>();
        }

        private void RegisterModelSerializers(ContainerBuilder builder, FileType fileType)
        {
            builder.RegisterType<JsonlModelSerializerService>().As<IModelSerializer<JsonlFileModel>>();
            builder.RegisterType<CustomTextEntitiesModelSerializerService>().As<IModelSerializer<CustomEntitiesFileModel>>();
        }

        private void RegisterModelConverters(ContainerBuilder builder, FileType sourceType, FileType targetType)
        {
            builder.RegisterType<JsonlModelConversionService>().As<IModelConverter<JsonlFileModel, CustomEntitiesFileModel>>();
        }

        private void RegisterConversionOrchestrator(ContainerBuilder builder, FileType sourceType, FileType targetType)
        {
            builder.RegisterType<FileConversionOrchestrator<JsonlFileModel, CustomEntitiesFileModel>>().As<IFileConversionOrchestrator>();
        }
    }
}
