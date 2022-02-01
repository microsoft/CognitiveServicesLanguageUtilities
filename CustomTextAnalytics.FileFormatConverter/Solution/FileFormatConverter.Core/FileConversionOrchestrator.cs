using FileFormatConverter.Core.Interfaces;

namespace FileFormatConverter.Orchestrators
{
    public class FileConversionOrchestrator<TSourceModel, TTargetModel>
    {
        private static IFileHandler _fileReaderService;
        private static IModelSerializer<TSourceModel> _sourceModelSerializerService;
        private static IModelConverter<TSourceModel, TTargetModel> _modelConverterService;
        private static IModelSerializer<TTargetModel> _targetModelSerializerService;

        public static void ConvertModelFile(string inputFilePath, string targetFilePath)
        {
            // read input file
            var fileContent = _fileReaderService.ReadFileAsString(inputFilePath);

            // parse file
            var sourceModel = _sourceModelSerializerService.Deserialize(fileContent);

            // convert model
            var targetModel = _modelConverterService.ConvertModel(sourceModel);

            // serialize model
            var serializedTargetModel = _targetModelSerializerService.Serialize(targetModel);

            // save output
            _fileReaderService.WriteFileAsString(targetFilePath, serializedTargetModel);
        }
    }
}
