using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces;
using Newtonsoft.Json;

namespace FileFormatConverter.Core.Services
{
    public class CustomTextEntitiesFileModelSerializer : IModelSerializer<CustomEntitiesFileModel>
    {
        public CustomEntitiesFileModel Deserialize(string content)
        {
            throw new System.NotImplementedException();
        }

        public string Serialize(CustomEntitiesFileModel model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}