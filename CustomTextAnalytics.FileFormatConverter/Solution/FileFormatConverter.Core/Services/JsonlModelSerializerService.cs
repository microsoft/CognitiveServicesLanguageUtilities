using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Models.Input.Jsonl;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FileFormatConverter.Orchestrators
{
    internal class JsonlModelSerializerService : IModelSerializer<JsonlFileModel>
    {
        /// <summary>
        /// docs: https://stackoverflow.com/questions/29729063/line-delimited-json-serializing-and-de-serializing
        /// </summary>
        public JsonlFileModel Deserialize(string content)
        {

            var result = new List<SingleLineContent>();
            var jsonReader = new JsonTextReader(new StringReader(content))
            {
                SupportMultipleContent = true // This is important!
            };

            var jsonSerializer = new JsonSerializer();
            while (jsonReader.Read())
            {
                var line = jsonSerializer.Deserialize<SingleLineContent>(jsonReader);
                result.Add(line);
            }

            return new JsonlFileModel()
            {
                lines = result.ToArray()
            };
        }

        public string Serialize(JsonlFileModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}