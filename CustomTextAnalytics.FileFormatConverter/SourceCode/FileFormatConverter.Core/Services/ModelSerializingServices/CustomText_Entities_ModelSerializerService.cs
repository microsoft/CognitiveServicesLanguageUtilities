using FileFormatConverter.Core.DataStructures.FileModels;
using FileFormatConverter.Core.Interfaces.Services;
using Newtonsoft.Json;

namespace FileFormatConverter.Core.Services.ModelSerializingServices
{
    public class CustomText_Entities_ModelSerializerService : IModelSerializer<CustomText_Entities_FileModel>
    {
        public CustomText_Entities_FileModel Deserialize(string content)
        {
            throw new System.NotImplementedException();
        }

        public string Serialize(CustomText_Entities_FileModel model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}