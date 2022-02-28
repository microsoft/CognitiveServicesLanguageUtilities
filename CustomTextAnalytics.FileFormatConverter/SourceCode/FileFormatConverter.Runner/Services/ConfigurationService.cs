using Autofac;
using FileFormatConverter.Core;
using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll;
using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Jsonl;
using FileFormatConverter.Core.DataStructures.FileModels.CustomText.Entities;
using FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel;
using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Core.Interfaces.Services;
using FileFormatConverter.Core.Services;
using FileFormatConverter.Core.Services.ModelConversionServices;
using FileFormatConverter.Core.Services.ModelSerializingServices;
using FileFormatConverter.Runner.DataStructures;
using System;

namespace FileFormatConverter.Runner.Services
{
    public class ConfigurationService
    {
        public IContainer RegisterServices(FileType sourceFileType, FileType targetFileType)
        {
            var builder = new ContainerBuilder();

            RegsiterFileHandler(builder);
            RegisterModelSerializers(builder);
            RegisterModelConverters(builder);
            RegisterOrchestrator(builder, sourceFileType, targetFileType);

            return builder.Build();
        }

        private void RegsiterFileHandler(ContainerBuilder builder)
        {
            builder.RegisterType<LocalFileHandlerService>().As<IFileHandler>();
        }

        private void RegisterModelSerializers(ContainerBuilder builder)
        {
            builder.RegisterType<AzureML_Jsonl_ModelSerializerService>().As<IModelSerializer<AzureML_Jsonl_FileModel>>();
            builder.RegisterType<CustomText_Entities_ModelSerializerService>().As<IModelSerializer<CustomText_Entities_FileModel>>();
            //builder.RegisterType<AzureML_Conll_ModelSerializerService>().As<IModelSerializer<AzureML_Conll_FileModel>>();
        }

        private void RegisterModelConverters(ContainerBuilder builder)
        {
            builder.RegisterType<AzureML_Jsonl_ModelConversionService>().As<IModelConverter<AzureML_Jsonl_FileModel, IntermediateEntitiesModel>>();
            builder.RegisterType<CustomText_Entities_ModelConversionService>().As<IModelConverter<CustomText_Entities_FileModel, IntermediateEntitiesModel>>();
            //builder.RegisterType<AzureML_Conll_ModelConversionService>().As<IModelConverter<AzureML_Conll_FileModel, IntermediateEntitiesModel>>();
        }

        private void RegisterOrchestrator(ContainerBuilder builder, FileType sourceType, FileType targetType)
        {
            if (sourceType == FileType.JSONL && targetType == FileType.CT_ENTITIES)
            {
                builder.RegisterType<FileConversionOrchestrator<AzureML_Jsonl_FileModel, IntermediateEntitiesModel, CustomText_Entities_FileModel>>().As<IFileConversionOrchestrator>();
            }
            /*else if (sourceType == FileType.CONLL && targetType == FileType.CT_ENTITIES)
            {
                builder.RegisterType<FileConversionOrchestrator<AzureML_Conll_FileModel, IntermediateEntitiesModel, CustomText_Entities_FileModel>>().As<IFileConversionOrchestrator>();
            }*/
            else
            {
                throw new Exception("Conversion not supported!");
            }
        }
    }
}
