using FileFormatConverter.Models;
using FileFormatConverter.Models.Input.Jsonl;

namespace FileFormatConverter.Orchestrators
{
    internal class ModelConversionService
    {
        public static CustomEntitiesFileModel ConvertModel(List<JsonlFileModel> jsonlContent)
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

        private static IEnumerable<string> ExtractEntityNames(List<JsonlFileModel> jsonlContent)
        {
            return jsonlContent.SelectMany(file =>
            {
                return file.Label.Select(label => label.Text);
            }).Distinct();
        }

        private static Dictionary<string, int> CreateEntitiesMap(IEnumerable<string> allEntityNames)
        {
            var allEntitiesMap = new Dictionary<string, int>();
            var tmp = allEntityNames.ToArray();
            for (var i = 0; i < tmp.Length; i++)
            {
                allEntitiesMap[tmp[i]] = i;
            }
            return allEntitiesMap;
        }

        private static IEnumerable<EntityDocument> ConvertDocuments(List<JsonlFileModel> jsonlContent, Dictionary<string, int> allEntitiesMap)
        {
            return jsonlContent.Select(inputDoc =>
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