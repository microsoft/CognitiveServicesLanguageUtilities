namespace FileFormatConverter.Orchestrators
{
    public class FileConversionOrchestrator
    {
        private FileReaderService _fileReaderService;
        private FileParserService _fileParserService;
        private ModelConversionService _modelConversionService;
        private FileSerializingService _fileSerializingService;
        private FileWriterService _fileWriterService;

        public static void ConvertModelFile(string inputFilePath, string targetFilePath)
        {
            // read input file
            var fileContent = FileReaderService.Load(inputFilePath);

            // parse file
            var parsedContent = FileParserService.Parse(fileContent);

            // convert model
            var convertedModel = ModelConversionService.ConvertModel(parsedContent);

            // serialize model
            var serializedModel = FileSerializingService.SerializeModel(convertedModel);

            // save output
            FileWriterService.WriteToFile(targetFilePath, serializedModel);
        }
    }
}
