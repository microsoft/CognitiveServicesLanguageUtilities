using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileFormatConverter.Core.Services.ModelSerializingServices
{
    public class AzureML_Jsonl_ModelSerializerService : IModelSerializer<AzureML_Jsonl_FileModel>
    {
        /// <summary>
        /// docs: https://stackoverflow.com/questions/29729063/line-delimited-json-serializing-and-de-serializing
        /// </summary>
        public AzureML_Jsonl_FileModel Deserialize(string content)
        {
            try
            {
                var jsonReader = new JsonTextReader(new StringReader(content))
                {
                    SupportMultipleContent = true // This is important!
                };

                var jsonSerializer = new JsonSerializer();
                var result = new List<SingleLineContent>();
                while (jsonReader.Read())
                {
                    var line = jsonSerializer.Deserialize<SingleLineContent>(jsonReader);
                    result.Add(line);
                }

                return new AzureML_Jsonl_FileModel()
                {
                    lines = result.ToArray()
                };
            }
            catch (Exception)
            {
                throw new Exception("Invalid Jsonl file format!");
            }
        }

        public string Serialize(AzureML_Jsonl_FileModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}