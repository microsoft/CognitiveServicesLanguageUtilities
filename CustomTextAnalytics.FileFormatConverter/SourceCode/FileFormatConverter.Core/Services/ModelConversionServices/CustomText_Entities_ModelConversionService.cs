using FileFormatConverter.Core.DataStructures.FileModels.CustomText.Entities;
using FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel;
using FileFormatConverter.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace FileFormatConverter.Core.Services.ModelConversionServices
{
    public class CustomText_Entities_ModelConversionService : IModelConverter<CustomText_Entities_FileModel, IntermediateEntitiesModel>
    {
        public IntermediateEntitiesModel ConvertToIntermediate(CustomText_Entities_FileModel sourceModel)
        {
            var entityNames = sourceModel.EntityNames;
            var documents = ConvertDocuments(sourceModel);
            return new IntermediateEntitiesModel()
            {
                EntityNames = entityNames,
                Documents = documents.ToArray(),
            };
        }

        public CustomText_Entities_FileModel ConvertFromIntermediate(IntermediateEntitiesModel intermediateModel)
        {
            var entityNames = intermediateModel.EntityNames;
            var documents = ConvertDocuments(intermediateModel);
            return new CustomText_Entities_FileModel()
            {
                EntityNames = entityNames,
                Documents = documents.ToArray(),
            };
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomDocument> ConvertDocuments(IntermediateEntitiesModel intermediateModel)
        {
            return intermediateModel.Documents.Select(document =>
            {
                var entities = ConvertEntities(document);
                return new DataStructures.FileModels.CustomText.Entities.CustomDocument()
                {
                    Location = document.Location,
                    Culture = document.Culture,
                    Entities = entities.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.IntermediateEntitiesModel.CustomDocument> ConvertDocuments(CustomText_Entities_FileModel intermediateModel)
        {
            return intermediateModel.Documents.Select(document =>
            {
                var entities = ConvertEntities(document);
                return new DataStructures.FileModels.IntermediateEntitiesModel.CustomDocument()
                {
                    Location = document.Location,
                    Culture = document.Culture,
                    Entities = entities.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomEntity> ConvertEntities(DataStructures.FileModels.IntermediateEntitiesModel.CustomDocument document)
        {
            return document.Entities.Select(entity =>
            {
                var labels = ConvertLabels(entity);
                return new DataStructures.FileModels.CustomText.Entities.CustomEntity()
                {
                    RegionStart = entity.RegionStart,
                    RegionLength = entity.RegionLength,
                    Labels = labels.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.IntermediateEntitiesModel.CustomEntity> ConvertEntities(DataStructures.FileModels.CustomText.Entities.CustomDocument document)
        {
            return document.Entities.Select(entity =>
            {
                var labels = ConvertLabels(entity);
                return new DataStructures.FileModels.IntermediateEntitiesModel.CustomEntity()
                {
                    RegionStart = entity.RegionStart,
                    RegionLength = entity.RegionLength,
                    Labels = labels.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomLabel> ConvertLabels(DataStructures.FileModels.IntermediateEntitiesModel.CustomEntity entity)
        {
            return entity.Labels.Select(label =>
            {
                return new DataStructures.FileModels.CustomText.Entities.CustomLabel()
                {
                    Entity = label.Entity,
                    Start = label.Start,
                    Length = label.Length
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.IntermediateEntitiesModel.CustomLabel> ConvertLabels(DataStructures.FileModels.CustomText.Entities.CustomEntity entity)
        {
            return entity.Labels.Select(label =>
            {
                return new DataStructures.FileModels.IntermediateEntitiesModel.CustomLabel()
                {
                    Entity = label.Entity,
                    Start = label.Start,
                    Length = label.Length
                };
            });
        }
    }
}