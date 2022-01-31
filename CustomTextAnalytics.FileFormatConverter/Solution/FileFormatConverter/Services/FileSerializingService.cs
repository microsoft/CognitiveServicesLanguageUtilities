using FileFormatConverter.Models;
using Newtonsoft.Json;

namespace FileFormatConverter.Orchestrators
{
    internal class FileSerializingService
    {
        public static string SerializeModel(CustomEntitiesFileModel model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}