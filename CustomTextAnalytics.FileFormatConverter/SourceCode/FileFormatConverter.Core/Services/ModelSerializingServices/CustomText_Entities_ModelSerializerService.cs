using FileFormatConverter.Core.DataStructures.FileModels.CustomText.Entities;
using FileFormatConverter.Core.Interfaces.Services;
using Newtonsoft.Json;
using System;

namespace FileFormatConverter.Core.Services.ModelSerializingServices
{
    public class CustomText_Entities_ModelSerializerService : IModelSerializer<CustomText_Entities_FileModel>
    {
        public CustomText_Entities_FileModel Deserialize(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<CustomText_Entities_FileModel>(content);
            }
            catch (Exception)
            {
                throw new Exception("Invalid Json file format");
            }
        }

        public string Serialize(CustomText_Entities_FileModel model)
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