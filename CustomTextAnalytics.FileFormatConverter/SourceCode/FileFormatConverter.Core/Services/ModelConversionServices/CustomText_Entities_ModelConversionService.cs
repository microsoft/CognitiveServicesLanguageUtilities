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
            var entityNames = GetExtractors(sourceModel);
            var documents = ConvertDocuments(sourceModel);
            return new IntermediateEntitiesModel()
            {
                Extractors = entityNames.ToArray(),
                Documents = documents.ToArray(),
            };
        }

        public CustomText_Entities_FileModel ConvertFromIntermediate(IntermediateEntitiesModel intermediateModel)
        {
            var entityNames = GetExtractors(intermediateModel);
            var documents = ConvertDocuments(intermediateModel);
            return new CustomText_Entities_FileModel()
            {
                Extractors = entityNames.ToArray(),
                Documents = documents.ToArray(),
            };
        }

        private IEnumerable<DataStructures.FileModels.IntermediateEntitiesModel.CustomExtractorInfo> GetExtractors(CustomText_Entities_FileModel sourceModel)
        {
            return sourceModel.Extractors
                            .Select(e =>
                            {
                                return new DataStructures.FileModels.IntermediateEntitiesModel.CustomExtractorInfo()
                                {
                                    Name = e.Name,
                                };
                            });
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomExtractorInfo> GetExtractors(IntermediateEntitiesModel sourceModel)
        {
            return sourceModel.Extractors
                            .Select(e =>
                            {
                                return new DataStructures.FileModels.CustomText.Entities.CustomExtractorInfo()
                                {
                                    Name = e.Name,
                                };
                            });
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomDocument> ConvertDocuments(IntermediateEntitiesModel intermediateModel)
        {
            return intermediateModel.Documents.Select(document =>
            {
                var entities = ConvertEntities(document);
                return new DataStructures.FileModels.CustomText.Entities.CustomDocument()
                {
                    Location = document.Location,
                    Language = document.Language,
                    Extractors = entities.ToArray()
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
                    Language = document.Language,
                    Extractors = entities.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomExtractor> ConvertEntities(DataStructures.FileModels.IntermediateEntitiesModel.CustomDocument document)
        {
            return document.Extractors.Select(entity =>
            {
                var labels = ConvertLabels(entity);
                return new DataStructures.FileModels.CustomText.Entities.CustomExtractor()
                {
                    RegionOffset = entity.RegionOffset,
                    RegionLength = entity.RegionLength,
                    Labels = labels.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.IntermediateEntitiesModel.CustomExtractor> ConvertEntities(DataStructures.FileModels.CustomText.Entities.CustomDocument document)
        {
            return document.Extractors.Select(entity =>
            {
                var labels = ConvertLabels(entity);
                return new DataStructures.FileModels.IntermediateEntitiesModel.CustomExtractor()
                {
                    RegionOffset = entity.RegionOffset,
                    RegionLength = entity.RegionLength,
                    Labels = labels.ToArray()
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.CustomText.Entities.CustomLabel> ConvertLabels(DataStructures.FileModels.IntermediateEntitiesModel.CustomExtractor entity)
        {
            return entity.Labels.Select(label =>
            {
                return new DataStructures.FileModels.CustomText.Entities.CustomLabel()
                {
                    ExtractorName = label.ExtractorName,
                    Offset = label.Offset,
                    Length = label.Length
                };
            });
        }

        private IEnumerable<DataStructures.FileModels.IntermediateEntitiesModel.CustomLabel> ConvertLabels(DataStructures.FileModels.CustomText.Entities.CustomExtractor entity)
        {
            return entity.Labels.Select(label =>
            {
                return new DataStructures.FileModels.IntermediateEntitiesModel.CustomLabel()
                {
                    ExtractorName = label.ExtractorName,
                    Offset = label.Offset,
                    Length = label.Length
                };
            });
        }
    }
}