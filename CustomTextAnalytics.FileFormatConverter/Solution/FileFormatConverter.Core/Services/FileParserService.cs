using FileFormatConverter.Models.Input.Jsonl;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FileFormatConverter.Orchestrators
{
    internal class FileParserService
    {
        // docs: https://stackoverflow.com/questions/29729063/line-delimited-json-serializing-and-de-serializing
        public static List<JsonlFileModel> Parse(string fileContent)
        {
            var result = new List<JsonlFileModel>();
            var jsonReader = new JsonTextReader(new StringReader(fileContent))
            {
                SupportMultipleContent = true // This is important!
            };

            var jsonSerializer = new JsonSerializer();
            while (jsonReader.Read())
            {
                var line = jsonSerializer.Deserialize<JsonlFileModel>(jsonReader);
                result.Add(line);
            }
            return result;
        }
    }
}