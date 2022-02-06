using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces.Services;
using Newtonsoft.Json;
using System;

namespace FileFormatConverter.Core.Services.ModelSerializingServices
{
    internal class CustomText_Classifiers_ModelSerializerService
    {
        public class CustomText_Entities_ModelSerializerService : IModelSerializer<CustomText_Classification_FileModel>
        {
            public CustomText_Classification_FileModel Deserialize(string content)
            {
                try
                {
                    return JsonConvert.DeserializeObject<CustomText_Classification_FileModel>(content);
                }
                catch (Exception)
                {
                    throw new Exception("Invalid Json file format");
                }
            }

            public string Serialize(CustomText_Classification_FileModel model)
            {
                try
                {
                    return JsonConvert.SerializeObject(model, Formatting.Indented);
                }
                catch (Exception)
                {
                    throw new Exception("Something went wrong went writing to target file!");
                }
            }
        }
    }
}
