using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Models;
using Newtonsoft.Json;

namespace FileFormatConverter.Orchestrators
{
    internal class CustomTextEntitiesFileModelSerializer : IModelSerializer<CustomEntitiesFileModel>
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