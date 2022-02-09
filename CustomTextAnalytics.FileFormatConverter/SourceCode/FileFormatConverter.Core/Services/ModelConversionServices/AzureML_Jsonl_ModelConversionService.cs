using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Jsonl;
using FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel;
using FileFormatConverter.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace FileFormatConverter.Core.Services.ModelConversionServices
{
    public class AzureML_Jsonl_ModelConversionService : IModelConverter<AzureML_Jsonl_FileModel, IntermediateEntitiesModel>
    {
        public IntermediateEntitiesModel ConvertToIntermediate(AzureML_Jsonl_FileModel jsonlContent)
        {
            // extract entity names (distinct)
            var allEntityNames = GetExtractors(jsonlContent);

            // each file
            var docsList = ConvertDocuments(jsonlContent);

            // final result
            return new IntermediateEntitiesModel()
            {
                Extractors = allEntityNames.ToArray(),
                Documents = docsList.ToArray()
            };
        }

        public AzureML_Jsonl_FileModel ConvertFromIntermediate(IntermediateEntitiesModel intermediateModel)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<CustomExtractorInfo> GetExtractors(AzureML_Jsonl_FileModel jsonlContent)
        {
            return jsonlContent.lines
                .SelectMany(file =>
                {
                    return file.Label.Select(label => label.Text);
                })
                .ToHashSet()
                .Select(text =>
                {
                    return new CustomExtractorInfo() { Name = text };
                });
        }

        private IEnumerable<CustomDocument> ConvertDocuments(AzureML_Jsonl_FileModel jsonlContent)
        {
            return jsonlContent.lines.Select(inputDoc =>
            {
                // map labels
                var resLabels = inputDoc.Label.Select(label =>
                {
                    return new CustomLabel()
                    {
                        ExtractorName = label.Text,
                        Offset = label.OffsetStart,
                        Length = label.OffsetEnd - label.OffsetStart
                    };
                }).ToArray();

                // res document
                return new CustomDocument()
                {
                    Location = inputDoc.ImageUrl,
                    Extractors = new CustomExtractor[]
                    {
                        new CustomExtractor()
                        {
                            Labels = resLabels
                        }
                    }
                };
            });
        }
    }
}