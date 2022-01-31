using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Services;

namespace FileFormatConverter.Orchestrators
{
    public class FileConversionOrchestrator
    {
        private static IFileHandlingService _fileReaderService = new LocalFileHandlerService();
        private FileParserService _fileParserService;
        private ModelConversionService _modelConversionService;
        private FileSerializingService _fileSerializingService;

        public static void ConvertModelFile(string inputFilePath, string targetFilePath)
        {
            // read input file
            var fileContent = _fileReaderService.ReadFile(inputFilePath);

            // parse file
            var parsedContent = FileParserService.Parse(fileContent);

            // convert model
            var convertedModel = ModelConversionService.ConvertModel(parsedContent);

            // serialize model
            var serializedModel = FileSerializingService.SerializeModel(convertedModel);

            // save output
            _fileReaderService.WriteFile(targetFilePath, serializedModel);
        }
    }
}
