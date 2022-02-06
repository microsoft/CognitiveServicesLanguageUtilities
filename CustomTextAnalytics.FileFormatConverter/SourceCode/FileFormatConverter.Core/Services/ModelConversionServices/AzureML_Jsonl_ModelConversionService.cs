using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace FileFormatConverter.Core.Services.ModelConversionServices
{
    public class AzureML_Jsonl_ModelConversionService : IModelConverter<AzureML_Jsonl_FileModel, CustomText_Entities_FileModel>
    {
        public CustomText_Entities_FileModel ConvertModel(AzureML_Jsonl_FileModel jsonlContent)
        {
            // extract entity names (distinct)
            var allEntityNames = ExtractEntityNames(jsonlContent);

            // create entity names map
            var allEntitiesMap = CreateEntitiesMap(allEntityNames);

            // each file
            var docsList = ConvertDocuments(jsonlContent, allEntitiesMap);

            // final result
            return new CustomText_Entities_FileModel()
            {
                EntityNames = allEntityNames.ToArray(),
                Documents = docsList.ToArray()
            };
        }

        private IEnumerable<string> ExtractEntityNames(AzureML_Jsonl_FileModel jsonlContent)
        {
            return jsonlContent.lines.SelectMany(file =>
            {
                return file.Label.Select(label => label.Text);
            }).Distinct();
        }

        private Dictionary<string, int> CreateEntitiesMap(IEnumerable<string> allEntityNames)
        {
            var allEntitiesMap = new Dictionary<string, int>();
            var tmp = allEntityNames.ToArray();
            for (var i = 0; i < tmp.Length; i++)
            {
                allEntitiesMap[tmp[i]] = i;
            }
            return allEntitiesMap;
        }

        private IEnumerable<EntityDocument> ConvertDocuments(AzureML_Jsonl_FileModel jsonlContent, Dictionary<string, int> allEntitiesMap)
        {
            return jsonlContent.lines.Select(inputDoc =>
            {
                // map labels
                var resLabels = inputDoc.Label.Select(label =>
                {
                    return new CustomLabel()
                    {
                        Entity = allEntitiesMap[label.Text],
                        Start = label.OffsetStart,
                        Length = label.OffsetEnd - label.OffsetStart
                    };
                }).ToArray();

                // res document
                return new EntityDocument()
                {
                    Location = inputDoc.ImageUrl,
                    Entities = new CustomEntity[]
                    {
                        new CustomEntity()
                        {
                            Labels = resLabels
                        }
                    }
                };
            });
        }
    }
}