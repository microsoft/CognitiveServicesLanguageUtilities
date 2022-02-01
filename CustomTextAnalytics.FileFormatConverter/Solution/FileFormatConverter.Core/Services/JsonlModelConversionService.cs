using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Models;
using FileFormatConverter.Models.Input.Jsonl;
using System.Collections.Generic;
using System.Linq;

namespace FileFormatConverter.Orchestrators
{
    public class JsonlModelConversionService : IModelConverter<JsonlFileModel, CustomEntitiesFileModel>
    {
        public CustomEntitiesFileModel ConvertModel(JsonlFileModel jsonlContent)
        {
            // extract entity names (distinct)
            var allEntityNames = ExtractEntityNames(jsonlContent);

            // create entity names map
            var allEntitiesMap = CreateEntitiesMap(allEntityNames);

            // each file
            var docsList = ConvertDocuments(jsonlContent, allEntitiesMap);

            // final result
            return new CustomEntitiesFileModel()
            {
                EntityNames = allEntityNames.ToArray(),
                Documents = docsList.ToArray()
            };
        }

        private IEnumerable<string> ExtractEntityNames(JsonlFileModel jsonlContent)
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

        private IEnumerable<EntityDocument> ConvertDocuments(JsonlFileModel jsonlContent, Dictionary<string, int> allEntitiesMap)
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