using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Services;

namespace FileFormatConverter.Orchestrators
{
    public class FileConversionOrchestrator
    {
        private static IFileHandler _fileReaderService = new LocalFileHandlerService();
        private static IModelSerializer _modelSerializerService = new ModelSerializerService();
        
        private static FileParserService _fileParserService;
        private static ModelConversionService _modelConversionService;

        public static void ConvertModelFile<TSourceModel, TTargetModel>(string inputFilePath, string targetFilePath)
        {
            // read input file
            var fileContent = _fileReaderService.ReadFile(inputFilePath); // done

            // parse file
            var tmp = _modelSerializerService.Deserialize<TSourceModel>(fileContent);

            // convert model
            var convertedModel = ModelConversionService.ConvertModel(parsedContent);

            // serialize model
            var serializedModel = _modelSerializerService.Serialize(convertedModel);

            // save output
            _fileReaderService.WriteFile(targetFilePath, serializedModel); // done
        }
    }
}
