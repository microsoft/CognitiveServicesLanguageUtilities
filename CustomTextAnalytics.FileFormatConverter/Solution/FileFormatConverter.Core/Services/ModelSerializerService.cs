using FileFormatConverter.Core.Interfaces;
using Newtonsoft.Json;

namespace FileFormatConverter.Orchestrators
{
    internal class ModelSerializerService : IModelSerializer
    {
        public string Serialize<T>(T model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
        public T Deserialize<T>(string content)
        {
            throw new System.NotImplementedException();
        }
    }
}