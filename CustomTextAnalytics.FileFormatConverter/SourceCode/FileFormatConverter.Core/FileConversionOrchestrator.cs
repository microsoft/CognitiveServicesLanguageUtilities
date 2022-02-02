using FileFormatConverter.Core.Interfaces;

namespace FileFormatConverter.Core
{
    public class FileConversionOrchestrator<TSourceModel, TTargetModel> : IFileConversionOrchestrator
    {
        private IFileHandler _fileHandlingService;
        private IModelSerializer<TSourceModel> _sourceModelSerializerService;
        private IModelConverter<TSourceModel, TTargetModel> _modelConverterService;
        private IModelSerializer<TTargetModel> _targetModelSerializerService;

        public FileConversionOrchestrator(
            IFileHandler fileReaderService,
            IModelSerializer<TSourceModel> sourceModelSerializerService,
            IModelConverter<TSourceModel, TTargetModel> modelConverterService,
            IModelSerializer<TTargetModel> targetModelSerializerService)
        {
            _fileHandlingService = fileReaderService;
            _sourceModelSerializerService = sourceModelSerializerService;
            _modelConverterService = modelConverterService;
            _targetModelSerializerService = targetModelSerializerService;
        }

        public void ConvertFile(string inputFilePath, string targetFilePath)
        {
            // read input file
            var fileContent = _fileHandlingService.ReadFileAsString(inputFilePath);

            // parse file
            var sourceModel = _sourceModelSerializerService.Deserialize(fileContent);

            // convert model
            var targetModel = _modelConverterService.ConvertModel(sourceModel);

            // serialize model
            var serializedTargetModel = _targetModelSerializerService.Serialize(targetModel);

            // save output
            _fileHandlingService.WriteFileAsString(targetFilePath, serializedTargetModel);
        }
    }
}
